using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region Readers

        public async Task<ActionResponse<PlanDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Plan>()
                    .FindBy(c => c.Id == id, includeProperties: "PlanDays.Subjects.PlanDaySubjectThemes");
                return await ActionResponse<PlanDto>
                    .ReturnSuccess(mapper.Map<Plan, PlanDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom dohvaćanja plana.");
            }
        }

        public async Task<ActionResponse<List<PlanDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Plan>()
                    .GetAll(includeProperties: "PlanDays.Subjects.PlanDaySubjectThemes");
                return await ActionResponse<List<PlanDto>>
                    .ReturnSuccess(mapper.Map<List<Plan>, List<PlanDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<PlanDto>>.ReturnError("Greška prilikom dohvaćanja planova.");
            }
        }

        public async Task<ActionResponse<PagedResult<PlanDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<Plan>()
                    .GetAllAsQueryable(includeProperties: "PlanDays.Subjects.PlanDaySubjectThemes").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<PlanDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<Plan>, List<PlanDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<PlanDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<PlanDto>>.ReturnError("Greška prilikom dohvaćanja straničnih podataka plana.");
            }
        }

        public async Task<ActionResponse<PlanDto>> GetByEducationProgramId(int educationPrograMid)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Plan>()
                    .FindBy(c => c.EducationProgramId == educationPrograMid, includeProperties: "PlanDays.Subjects.PlanDaySubjectThemes");
                return await ActionResponse<PlanDto>
                    .ReturnSuccess(mapper.Map<Plan, PlanDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom dohvaćanja plana.");
            }
        }

        #endregion Readers

        #region Modifiers

        public async Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto)
        {
            try
            {
                List<PlanDayDto> planDays = new List<PlanDayDto>(entityDto.PlanDays);
                entityDto.PlanDays = null;
                entityDto.PlanDaysId = null;

                var planToAdd = mapper.Map<PlanDto, Plan>(entityDto);
                unitOfWork.GetGenericRepository<Plan>().Add(planToAdd);
                unitOfWork.Save();
                mapper.Map(planToAdd, entityDto);

                entityDto.PlanDays = planDays.Select(pd =>
                {
                    pd.PlanId = entityDto.Id;
                    return pd;
                }).ToList();

                if ((await ModifyPlanDays(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDto> modifyDaysResponse, out entityDto))
                {
                    return modifyDaysResponse;
                }

                if ((await GetById(planToAdd.Id)).IsNotSuccess(out ActionResponse<PlanDto> getResponse, out entityDto))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Greška prilikom upisivanja podataka za plan.");
                }

                return await ActionResponse<PlanDto>.ReturnSuccess(entityDto, "Plan uspješno unesen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }

        public async Task<ActionResponse<PlanDto>> Update(PlanDto entityDto)
        {
            try
            {
                List<PlanDayDto> planDays = new List<PlanDayDto>(entityDto.PlanDays);
                entityDto.PlanDays = null;
                entityDto.PlanDaysId = null;

                var entityToUpdate = mapper.Map<PlanDto, Plan>(entityDto);
                unitOfWork.GetGenericRepository<Plan>().Update(entityToUpdate);
                unitOfWork.Save();

                entityDto.PlanDays = planDays;
                if ((await ModifyPlanDays(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDto> modifyDaysResponse, out entityDto))
                {
                    return modifyDaysResponse;
                }

                if ((await GetById(entityToUpdate.Id)).IsNotSuccess(out ActionResponse<PlanDto> getResponse, out entityDto))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Greška prilikom ažuriranja podataka za plan.");
                }

                return await ActionResponse<PlanDto>.ReturnSuccess(entityDto, "Plan uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom ažuriranja plana.");
            }
        }

        public async Task<ActionResponse<PlanDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Plan>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<PlanDto>.ReturnSuccess(null, "Brisanje plana uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom brisanja plana.");
            }
        }

        #endregion Modifiers

        #region Plan Days

        private async Task<ActionResponse<PlanDto>> ModifyPlanDays(PlanDto plan)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Plan>()
                    .FindBy(p => p.Id == plan.Id, includeProperties: "PlanDays.Subjects.PlanDaySubjectThemes.Theme");
                plan.PlanDaysId = null;

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

        #region RemoveDays

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

        #endregion RemoveDays

        #region Add Days

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

        public async Task<ActionResponse<PlanDayDto>> AddDayToPlan(PlanDayDto entityDto)
        {
            try
            {
                List<PlanDaySubjectDto> planDaySubjects = new List<PlanDaySubjectDto>(entityDto.PlanDaySubjects);
                entityDto.PlanDaySubjectIds = null;
                entityDto.PlanDaySubjects = null;

                var entityToAdd = mapper.Map<PlanDayDto, PlanDay>(entityDto);
                unitOfWork.GetGenericRepository<PlanDay>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                entityDto.PlanDaySubjects = planDaySubjects.Select(pd =>
                {
                    pd.PlanDayId = entityDto.Id;
                    return pd;
                }).ToList();

                if ((await ModifyPlanDaySubjects(entityDto)).IsNotSuccess(out ActionResponse<PlanDayDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<PlanDayDto>.ReturnSuccess(mapper.Map(entityToAdd, entityDto), "Dan plana uspješno unesen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }

        #endregion Add Days

        #region Modify Days

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
                List<PlanDaySubjectDto> planDaySubjects = new List<PlanDaySubjectDto>(entityDto.PlanDaySubjects);
                entityDto.PlanDaySubjectIds = null;
                entityDto.PlanDaySubjects = null;

                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDay>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDay>().Update(entityToUpdate);
                unitOfWork.Save();

                entityDto.PlanDaySubjects = planDaySubjects.Select(pd =>
                {
                    pd.PlanDayId = entityDto.Id;
                    return pd;
                }).ToList();

                if ((await ModifyPlanDaySubjects(entityDto)).IsNotSuccess(out ActionResponse<PlanDayDto> response, out entityDto))
                {
                    return response;
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

        #endregion Modify Days

        #region Plan Day Subjects

        public async Task<ActionResponse<PlanDayDto>> ModifyPlanDaySubjects(PlanDayDto entityDto)
        {
            var entity = unitOfWork.GetGenericRepository<PlanDay>()
                    .FindBy(p => p.Id == entityDto.Id, includeProperties: "Subjects.PlanDaySubjectThemes.Theme");
            entityDto.PlanDaySubjectIds = null;

            var currentSubjects = mapper.Map<List<PlanDaySubject>, List<PlanDaySubjectDto>>(entity.Subjects.ToList());

            var subjectsToRemove = currentSubjects.Where(cd => !entityDto.PlanDaySubjects
                .Select(pds => pds.SubjectId.Value)
                .Contains(cd.SubjectId.Value))
                .ToList();

            var subjectsToAdd = entityDto.PlanDaySubjects
                .Where(nt => !currentSubjects.Select(cd => cd.SubjectId.Value).Contains(nt.SubjectId.Value))
                .Select(pds => new PlanDaySubjectDto
                {
                    PlanDayId = entity.Id,
                    SubjectId = pds.SubjectId,
                    PlanDaySubjectThemes = pds.PlanDaySubjectThemes
                })
                .ToList();

            var subjectsToModify = entityDto.PlanDaySubjects.Where(cd => currentSubjects
                .Select(pds => pds.SubjectId.Value)
                .Contains(cd.SubjectId.Value))
                .ToList();

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

        #region Remove Plan Day Subject

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

        #endregion Remove Plan Day Subject

        #region Add Plan Day Subject

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
                List<PlanDaySubjectThemeDto> planDaySubjectThemes = new List<PlanDaySubjectThemeDto>(entityDto.PlanDaySubjectThemes);
                entityDto.PlanDaySubjectThemeIds = null;
                entityDto.PlanDaySubjectThemes = null;

                var entityToAdd = mapper.Map<PlanDaySubjectDto, PlanDaySubject>(entityDto);
                unitOfWork.GetGenericRepository<PlanDaySubject>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                entityDto.PlanDaySubjectThemes = planDaySubjectThemes.Select(pd =>
                {
                    pd.PlanDaySubjectId = entityDto.Id;
                    return pd;
                }).ToList();

                if ((await ModifyPlanDaySubjectsThemes(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectDto> themesResponse, out entityDto))
                {
                    return themesResponse;
                }

                return await ActionResponse<PlanDaySubjectDto>.ReturnSuccess(entityDto, "Predmet uspješno dodan u dan plana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Greška prilikom dodavanja predmeta u dan plana.");
            }
        }

        #endregion Add Plan Day Subject

        #region Modify Plan Day Subject

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
                List<PlanDaySubjectThemeDto> planDaySubjectThemes = new List<PlanDaySubjectThemeDto>(entityDto.PlanDaySubjectThemes);
                entityDto.PlanDaySubjectThemeIds = null;
                entityDto.PlanDaySubjectThemes = null;

                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDaySubject>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDaySubject>().Update(entityToUpdate);
                unitOfWork.Save();

                entityDto.PlanDaySubjectThemes = planDaySubjectThemes;
                if ((await ModifyPlanDaySubjectsThemes(entityDto))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectDto> themesResponse, out entityDto))
                {
                    return themesResponse;
                }

                return await ActionResponse<PlanDaySubjectDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Predmet u danu plana uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Greška prilikom ažuriranja predmeta u danu plana.");
            }
        }

        #endregion Modify Plan Day Subject

        #endregion Plan Day Subjects

        #region Plan Day Subjects Themes

        public async Task<ActionResponse<PlanDaySubjectDto>> ModifyPlanDaySubjectsThemes(PlanDaySubjectDto entityDto)
        {
            var entity = unitOfWork.GetGenericRepository<PlanDaySubject>()
                    .FindBy(p => p.Id == entityDto.Id, includeProperties: "PlanDaySubjectThemes.Theme");

            var currentThemes = mapper.Map<List<PlanDaySubjectTheme>, List<PlanDaySubjectThemeDto>>(entity.PlanDaySubjectThemes.ToList());

            var themesToRemove = currentThemes.Where(cd => !entityDto.PlanDaySubjectThemes
                .Select(pdst => pdst.ThemeId.Value)
                .Contains(cd.ThemeId.Value))
                .ToList();

            var themesToAdd = entityDto.PlanDaySubjectThemes
                .Where(nt => !currentThemes.Select(cd => cd.ThemeId.Value).Contains(nt.ThemeId.Value))
                .Select(pds => new PlanDaySubjectThemeDto
                {
                    ThemeId = pds.ThemeId.Value,
                    PlanDaySubjectId = entity.Id,
                    HoursNumber = pds.HoursNumber
                }).ToList();

            var themesToModify = currentThemes.Where(cd => entityDto.PlanDaySubjectThemes
            .Select(pdst => pdst.ThemeId.Value)
            .Contains(cd.ThemeId.Value)).ToList();

            if ((await AddThemesToPlanDaySubject(themesToAdd))
                .IsNotSuccess(out ActionResponse<List<PlanDaySubjectThemeDto>> actionResponse, out themesToAdd))
            {
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await ModifyThemesInPlanDaySubject(themesToModify)).IsNotSuccess(out actionResponse, out themesToModify))
            {
                return await ActionResponse<PlanDaySubjectDto>.ReturnError("Neuspješno ažuriranje predmeta u danu plana.");
            }

            if ((await RemoveThemesFromPlanDaySubject(themesToRemove))
                .IsNotSuccess(out actionResponse))
            {
                return await ActionResponse<PlanDaySubjectDto>.ReturnError(actionResponse.Message);
            }

            return await ActionResponse<PlanDaySubjectDto>.ReturnSuccess(mapper.Map(entity, entityDto), "Uspješno izmijenjeni dani plana.");
        }

        public async Task<ActionResponse<List<PlanDaySubjectThemeDto>>> RemoveThemesFromPlanDaySubject(List<PlanDaySubjectThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnSuccess(null, "Teme uspješno maknute iz predmeta u planskom danu.");
                entityDtos.ForEach(async pd =>
                {
                    if ((await RemoveThemeFromPlanDaySubject(pd))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectThemeDto> actionResponse))
                    {
                        response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError("Greška prilikom micanja teme iz predmeta u planskom danu.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectThemeDto>> RemoveThemeFromPlanDaySubject(PlanDaySubjectThemeDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<PlanDaySubjectTheme>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<PlanDaySubjectThemeDto>.ReturnSuccess(null, "Tema uspješno izbrisana iz predmeta planskog dana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectThemeDto>.ReturnError("Greška prilikom micanja teme iz predmeta planskog dana.");
            }
        }

        public async Task<ActionResponse<List<PlanDaySubjectThemeDto>>> AddThemesToPlanDaySubject(List<PlanDaySubjectThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnSuccess(entityDtos, "Teme uspješno dodane u dan plana.");
                entityDtos.ForEach(async pdt =>
                {
                    if ((await InsertPlanDaySubjectTheme(pdt))
                        .IsNotSuccess(out ActionResponse<PlanDaySubjectThemeDto> insertResponse, out pdt))
                    {
                        response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError("Greška prilikom dodavanja tema u predmet planskog dana.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectThemeDto>> InsertPlanDaySubjectTheme(PlanDaySubjectThemeDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDaySubjectThemeDto, PlanDaySubjectTheme>(entityDto);
                unitOfWork.GetGenericRepository<PlanDaySubjectTheme>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<PlanDaySubjectThemeDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto), "Tema uspješno dodana u predmet planskog dana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectThemeDto>.ReturnError("Greška prilikom dodavanja teme u predmet planskog dana.");
            }
        }


        public async Task<ActionResponse<List<PlanDaySubjectThemeDto>>> ModifyThemesInPlanDaySubject(List<PlanDaySubjectThemeDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnSuccess(null, "Teme u planskom danu uspješno ažurirane.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await ModifyThemeInPlanDay(pds))
                    .IsNotSuccess(out ActionResponse<PlanDaySubjectThemeDto> insertResponse, out pds))
                    {
                        response = await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDaySubjectThemeDto>>.ReturnError("Greška prilikom ažuriranja tema u predmetu planskog dana.");
            }
        }

        public async Task<ActionResponse<PlanDaySubjectThemeDto>> ModifyThemeInPlanDay(PlanDaySubjectThemeDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<PlanDaySubjectTheme>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<PlanDaySubjectTheme>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<PlanDaySubjectThemeDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Tema u danu plana uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDaySubjectThemeDto>.ReturnError("Greška prilikom ažuriranja teme u danu plana.");
            }
        }

        #endregion Plan Day Subjects Themes

        #endregion Plan Days
    }
}
