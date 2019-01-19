using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PlanService : IPlanService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto)
        {
            try
            {
                entityDto.PlanDays = null;
                var planToAdd = mapper.Map<PlanDto, Plan>(entityDto);
                unitOfWork.GetGenericRepository<Plan>().Add(planToAdd);
                unitOfWork.Save();

                mapper.Map(planToAdd, entityDto);
                return await ActionResponse<PlanDto>.ReturnSuccess(entityDto, "Plan uspješno upisan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> InsertPlanForEducationProgram(EducationProgramDto completeEduProgram)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var planDto = completeEduProgram.Plan;
                    List<PlanDayDto> planDays = new List<PlanDayDto>(planDto.PlanDays);
                    planDto.PlanDays = null;
                    if ((await Insert(completeEduProgram.Plan)).IsNotSuccess(out ActionResponse<PlanDto> planResponse, out planDto))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(planResponse.Message);
                    }

                    planDto.PlanDays = new List<PlanDayDto>(planDays);
                    if ((await PreparePlanForInsertFromEducationProgram(completeEduProgram))
                        .IsNotSuccess(out planResponse, out planDto))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(planResponse.Message);
                    }

                    if ((await ModifyPlanDaysForPlanInEducationProgram(planDto)).IsNotSuccess(out planResponse, out planDto))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(planResponse.Message);
                    }

                    scope.Complete();
                    return await ActionResponse<EducationProgramDto>.ReturnSuccess(completeEduProgram, "Plan uspješno upisan.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, completeEduProgram);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }

        private async Task<ActionResponse<PlanDto>> PreparePlanForInsertFromEducationProgram(EducationProgramDto educationProgram)
        {
            try
            {
                var planToPrepare = educationProgram.Plan;

                if (planToPrepare.PlanDays
                    .Any(pd => pd.PlanDaySubjects
                        .Any(pds => pds.Subject == null || string.IsNullOrEmpty(pds.Subject.Name)))
                        ||
                    planToPrepare.PlanDays
                    .Any(pd => pd.PlanDayThemes
                        .Any(pds => pds.Theme == null || string.IsNullOrEmpty(pds.Theme.Name))))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Dio podataka za dane plana nedostaje. " +
                        "Svi predmeti i sve teme moraju imati ime.");
                }

                var educationProgramSubjects = educationProgram.Subjects;
                var educationProgramThemes = educationProgram.Subjects
                    .SelectMany(s => s.Themes)
                        .GroupBy(t => t.Name)
                        .Select(g => g.FirstOrDefault())
                        .Where(t => t != null)
                        .ToList();

                planToPrepare.PlanDays
                .Select(pd =>
                {
                    pd.PlanId = planToPrepare.Id;
                    pd.PlanDaySubjectIds = educationProgramSubjects
                        .Where(s => pd.PlanDaySubjects.Any(pds => pds.Subject.Name == s.Name))
                        .Select(s => s.Id.Value)
                        .ToList();

                    pd.PlanDaySubjects.Select(pds =>
                    {
                        pds.SubjectId = educationProgramSubjects
                        .Where(s => s.Name == pds.Subject.Name).FirstOrDefault().Id;
                        return pds;
                    }).ToList();

                    pd.PlanDayThemeIds = educationProgramThemes
                        .Where(t => pd.PlanDayThemes.Any(pdt => pdt.Theme.Name == t.Name))
                        .Select(t => t.Id.Value)
                        .ToList();

                    pd.PlanDayThemes.Select(pdt =>
                    {
                        pdt.ThemeId = educationProgramThemes
                        .Where(s => s.Name == pdt.Theme.Name).FirstOrDefault().Id;
                        return pdt;
                    }).ToList();
                    return pd;
                })
                .ToList();

                return await ActionResponse<PlanDto>.ReturnSuccess(planToPrepare, "Plan uspješno pripremljen za unos ili modifikaciju.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, educationProgram);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom pripreme plana za unos.");
            }
        }

        private async Task<ActionResponse<PlanDto>> ModifyPlanDaysForPlanInEducationProgram(PlanDto plan)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Plan>()
                    .FindBy(p => p.Id == plan.Id, includeProperties: "PlanDays.Subjects,PlanDays.Themes");

                var currentDays = mapper.Map<List<PlanDay>, List<PlanDayDto>>(entity.PlanDays.ToList());

                var newDays = plan.PlanDays;

                var daysToRemove = currentDays.Where(cd => !newDays.Select(nd => nd.Id).Contains(cd.Id)).ToList();

                var daysToAdd = newDays
                    .Where(nt => !currentDays.Select(cd => cd.Id).Contains(nt.Id))
                    .ToList();

                var daysToModify = newDays.Where(cd => currentDays.Select(nd => nd.Id).Contains(cd.Id)).ToList();

                if ((await RemoveDaysFromPlan(daysToRemove))
                    .IsNotSuccess(out ActionResponse<List<PlanDayDto>> actionResponse))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Neuspješno ažuriranje dana u planu.");
                }

                if ((await AddDaysToPlan(daysToAdd)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Neuspješno ažuriranje dana u planu.");
                }

                if ((await ModifyDaysInPlan(daysToModify)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Neuspješno ažuriranje dana u planu.");
                }

                return await ActionResponse<PlanDto>.ReturnSuccess(plan, "Uspješno izmijenjeni dani plana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, plan);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom modifikacije dana za plan.");
            }
        }

        public async Task<ActionResponse<List<PlanDayDto>>> RemoveDaysFromPlan(List<PlanDayDto> daysInPlan)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayDto>>.ReturnSuccess(null, "Dani uspješno maknuti iz plana.");
                daysInPlan.ForEach(async pd =>
                {
                    if ((await RemoveDayFromPlan(pd))
                    .IsNotSuccess(out ActionResponse<PlanDayDto> actionResponse))
                    {
                        response = await ActionResponse<List<PlanDayDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, daysInPlan);
                return await ActionResponse<List<PlanDayDto>>.ReturnError("Greška prilikom micanja dana iz plana.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> RemoveDayFromPlan(PlanDayDto planDay)
        {
            try
            {
                unitOfWork.GetGenericRepository<PlanDay>().Delete(planDay.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<PlanDayDto>.ReturnSuccess(null, "Dan uspješno izbrisan iz plana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, planDay);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom micanja dana iz plana.");
            }
        }

        public async Task<ActionResponse<List<PlanDayDto>>> AddDaysToPlan(List<PlanDayDto> planDays)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayDto>>.ReturnSuccess(null, "Dani uspješno dodani u plan.");
                planDays.ForEach(async pd =>
                {
                    if ((await AddDayToPlan(pd))
                    .IsNotSuccess(out ActionResponse<PlanDayDto> actionResponse, out pd))
                    {
                        response = await ActionResponse<List<PlanDayDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, planDays);
                return await ActionResponse<List<PlanDayDto>>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }

        public async Task<ActionResponse<List<PlanDayDto>>> ModifyDaysInPlan(List<PlanDayDto> planDays)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayDto>>.ReturnSuccess(null, "Dani uspješno izmijenjeni.");
                planDays.ForEach(async pd =>
                {
                    if ((await UpdatePlanDay(pd))
                    .IsNotSuccess(out ActionResponse<PlanDayDto> actionResponse, out pd))
                    {
                        response = await ActionResponse<List<PlanDayDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, planDays);
                return await ActionResponse<List<PlanDayDto>>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> UpdatePlanDay(PlanDayDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDay>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDay>().Update(entityToUpdate);
                unitOfWork.Save();

                if ((await ModifyPlanDaySubjects(entityDto)).IsNotSuccess(out ActionResponse<PlanDayDto> response, out entityDto))
                {
                    return response;
                }

                if ((await ModifyPlanDayThemes(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDayDto> pdtResponse, out entityDto))
                {
                    return await ActionResponse<PlanDayDto>.ReturnError(pdtResponse.Message);
                }

                return await ActionResponse<PlanDayDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom ažuriranja dana plana.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> AddDayToPlan(PlanDayDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDayDto, PlanDay>(entityDto);
                unitOfWork.GetGenericRepository<PlanDay>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                if ((await ModifyPlanDaySubjects(entityDto)).IsNotSuccess(out ActionResponse<PlanDayDto> response, out entityDto))
                {
                    return response;
                }

                if ((await ModifyPlanDayThemes(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDayDto> pdtResponse, out entityDto))
                {
                    return await ActionResponse<PlanDayDto>.ReturnError(pdtResponse.Message);
                }

                return await ActionResponse<PlanDayDto>.ReturnSuccess(mapper.Map(entityToAdd, entityDto), "Dan plana uspješno unesen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> ModifyPlanDaySubjects(PlanDayDto entityDto)
        {
            var entity = unitOfWork.GetGenericRepository<PlanDay>()
                    .FindBy(p => p.Id == entityDto.Id, includeProperties: "Subjects");

            var currentSubjects = mapper.Map<List<PlanDaySubject>, List<PlanDaySubjectDto>>(entity.Subjects.ToList());

            var subjectsToRemove = currentSubjects.Where(cd => !entityDto.PlanDaySubjectIds.Contains(cd.SubjectId.Value)).ToList();

            var subjectsToAdd = entityDto.PlanDaySubjectIds
                .Where(nt => !currentSubjects.Select(cd => cd.SubjectId.Value).Contains(nt))
                .Select(pds => new PlanDaySubjectDto
                {
                    PlanDayId = entity.Id,
                    SubjectId = pds
                })
                .ToList();

            var subjectsToModify = currentSubjects.Where(cd => entityDto.PlanDaySubjectIds.Contains(cd.SubjectId.Value)).ToList();

            if ((await RemoveSubjectsFromPlanDay(subjectsToRemove))
                .IsNotSuccess(out ActionResponse<List<PlanDaySubjectDto>> actionResponse))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await AddSubjectsToPlanDay(subjectsToAdd)).IsNotSuccess(out ActionResponse<List<PlanDaySubjectDto>> subjectsResponse, out subjectsToAdd))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await ModifySubjectsInPlanDay(subjectsToModify)).IsNotSuccess(out actionResponse))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            return await ActionResponse<PlanDayDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dani plana.");
        }

        public async Task<ActionResponse<List<PlanDaySubjectDto>>> AddSubjectsToPlanDay(List<PlanDaySubjectDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnSuccess(entityDtos, "Predmeti uspješno dodani u dan plana.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await InsertPlanDaySubject(pds))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectDto> insertResponse, out pds))
                    {
                        response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectDto>>.ReturnError("Greška prilikom dodavanja predmeta u dan plana.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectDto>> InsertPlanDaySubject(PlanDaySubjectDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDaySubjectDto, PlanDaySubject>(entityDto);
                unitOfWork.GetGenericRepository<PlanDaySubject>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnSuccess(entityDto, "Predmet uspješno dodan u dan plana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Greška prilikom dodavanja predmeta u dan plana.");
            }
        }

        public async Task<ActionResponse<List<PlanDaySubjectDto>>> RemoveSubjectsFromPlanDay(List<PlanDaySubjectDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnSuccess(null, "Predmeti maknuti iz planskog dana.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await RemoveSubjectFromPlanDay(pds))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectDto> actionResponse))
                    {
                        response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectDto>>.ReturnError("Greška prilikom micanja predmeta iz dana plana.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectDto>> RemoveSubjectFromPlanDay(PlanDaySubjectDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<PlanDaySubject>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<PlanDaySubjectDto>.ReturnSuccess(null, "Predmet uspješno izbrisan iz planskog dana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Greška prilikom micanja predmeta iz planskog dana.");
            }
        }

        public async Task<ActionResponse<List<PlanDaySubjectDto>>> ModifySubjectsInPlanDay(List<PlanDaySubjectDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnSuccess(null, "Predmeti u planskom danu uspješno ažurirani.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await ModifyPlanDaySubject(pds))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectDto> insertResponse, out pds))
                    {
                        response = await ActionResponse<List<PlanDaySubjectDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectDto>>.ReturnError("Greška prilikom dodavanja predmeta u dan plana.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectDto>> ModifyPlanDaySubject(PlanDaySubjectDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDaySubject>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDaySubject>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<PlanDaySubjectDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Predmet u danu plana uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Greška prilikom ažuriranja predmeta u danu plana.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> ModifyPlanDayThemes(PlanDayDto entityDto)
        {
            var entity = unitOfWork.GetGenericRepository<PlanDay>()
                .FindBy(p => p.Id == entityDto.Id, includeProperties: "Themes");

            var currentThemes = mapper.Map<List<PlanDayTheme>, List<PlanDayThemeDto>>(entity.Themes.ToList());

            var themesToRemove = currentThemes.Where(cd => !entityDto.PlanDayThemeIds.Contains(cd.ThemeId.Value)).ToList();

            var themesToAdd = entityDto.PlanDayThemeIds
                .Where(nt => !currentThemes.Select(cd => cd.ThemeId.Value).Contains(nt))
                .Select(pds => new PlanDayThemeDto
                {
                    PlanDayId = entity.Id,
                    ThemeId = pds
                })
                .ToList();

            var themesToModify = currentThemes.Where(cd => entityDto.PlanDayThemeIds.Contains(cd.ThemeId.Value)).ToList();

            if ((await RemoveThemesFromPlanDay(themesToRemove))
                .IsNotSuccess(out ActionResponse<List<PlanDayThemeDto>> actionResponse))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await InsertPlanDayThemes(themesToAdd)).IsNotSuccess(out ActionResponse<List<PlanDayThemeDto>> subjectsResponse, out themesToAdd))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await ModifyThemesInPlanDay(themesToModify)).IsNotSuccess(out actionResponse))
            {
                return await ActionResponse<PlanDayDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            return await ActionResponse<PlanDayDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dani plana.");
        }

        public async Task<ActionResponse<List<PlanDayThemeDto>>> InsertPlanDayThemes(List<PlanDayThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayThemeDto>>.ReturnSuccess(entityDtos, "Teme uspješno dodane u dan plana.");
                entityDtos.ForEach(async pdt =>
                {
                    if ((await InsertPlanDayTheme(pdt))
                        .IsNotSuccess(out ActionResponse<PlanDayThemeDto> insertResponse, out pdt))
                    {
                        response = await ActionResponse<List<PlanDayThemeDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDayThemeDto>>.ReturnError("Greška prilikom dodavanja tema u dan plana.");
            }
        }

        public async Task<ActionResponse<PlanDayThemeDto>> InsertPlanDayTheme(PlanDayThemeDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDayThemeDto, PlanDayTheme>(entityDto);
                unitOfWork.GetGenericRepository<PlanDayTheme>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<PlanDayThemeDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto), "Tema uspješno dodana u dan plana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayThemeDto>.ReturnError("Greška prilikom dodavanja teme u dan plana.");
            }
        }

        public async Task<ActionResponse<List<PlanDayThemeDto>>> RemoveThemesFromPlanDay(List<PlanDayThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayThemeDto>>.ReturnSuccess(null, "Teme maknute iz planskog dana.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await RemoveThemeFromPlanDay(pds))
                    .IsNotSuccess(out ActionResponse<PlanDayThemeDto> actionResponse))
                    {
                        response = await ActionResponse<List<PlanDayThemeDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDayThemeDto>>.ReturnError("Greška prilikom micanja predmeta iz dana plana.");
            }
        }

        public async Task<ActionResponse<PlanDayThemeDto>> RemoveThemeFromPlanDay(PlanDayThemeDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<PlanDayTheme>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<PlanDayThemeDto>.ReturnSuccess(null, "Tema uspješno izbrisana iz planskog dana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayThemeDto>.ReturnError("Greška prilikom micanja teme iz planskog dana.");
            }
        }

        public async Task<ActionResponse<List<PlanDayThemeDto>>> ModifyThemesInPlanDay(List<PlanDayThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayThemeDto>>.ReturnSuccess(null, "Teme u planskom danu uspješno ažurirani.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await ModifyThemeInPlanDay(pds))
                    .IsNotSuccess(out ActionResponse<PlanDayThemeDto> insertResponse, out pds))
                    {
                        response = await ActionResponse<List<PlanDayThemeDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDayThemeDto>>.ReturnError("Greška prilikom dodavanja tema u dan plana.");
            }
        }

        public async Task<ActionResponse<PlanDayThemeDto>> ModifyThemeInPlanDay(PlanDayThemeDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDayTheme>().FindSingle(entityDto.Id.Value);
                mapper.Map<PlanDayThemeDto, PlanDayTheme>(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDayTheme>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<PlanDayThemeDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Tema u danu plana uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayThemeDto>.ReturnError("Greška prilikom ažuriranja teme u danu plana.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> UpdatePlanForEducationProgram(EducationProgramDto completeEduProgram)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var planDto = completeEduProgram.Plan;
                    List<PlanDayDto> planDays = new List<PlanDayDto>(planDto.PlanDays);
                    planDto.PlanDays = null;

                    planDto.PlanDays = new List<PlanDayDto>(planDays);
                    if ((await PreparePlanForInsertFromEducationProgram(completeEduProgram))
                        .IsNotSuccess(out ActionResponse<PlanDto> planResponse, out planDto))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(planResponse.Message);
                    }

                    if ((await ModifyPlanDaysForPlanInEducationProgram(planDto)).IsNotSuccess(out planResponse, out planDto))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(planResponse.Message);
                    }

                    scope.Complete();
                    return await ActionResponse<EducationProgramDto>.ReturnSuccess(completeEduProgram, "Plan uspješno upisan.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, completeEduProgram);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }
    }
}
