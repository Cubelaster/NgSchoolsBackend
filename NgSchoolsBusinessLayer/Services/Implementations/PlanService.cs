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
                    pd.PlanDayThemeIds = educationProgramThemes
                        .Where(t => pd.PlanDayThemes.Any(pdt => pdt.Theme.Name == t.Name))
                        .Select(t => t.Id.Value)
                        .ToList();
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
                var planDto = plan;
                var entity = unitOfWork.GetGenericRepository<Plan>()
                    .FindBy(p => p.Id == plan.Id, includeProperties: "PlanDay.Subjects,PlanDay.Themes");

                var currentDays = mapper.Map<List<PlanDay>, List<PlanDayDto>>(entity.PlanDays.ToList());

                var newDays = planDto.PlanDaysId;

                var daysToRemove = currentDays.Where(cd => !newDays.Contains(cd.Id.Value)).ToList();

                var daysToAdd = newDays
                    .Where(nt => !currentDays.Select(cd => cd.Id).Contains(nt))
                    .Select(nt => new PlanDayDto { PlanId = nt })
                    .ToList();

                if ((await RemoveDaysFromPlan(daysToRemove))
                    .IsNotSuccess(out ActionResponse<List<PlanDayDto>> actionResponse))
                {
                    return await ActionResponse<PlanDto>.ReturnError("Neuspješno ažuriranje dana u planu.");
                }

                if ((await AddDaysToPlan(daysToAdd)).IsNotSuccess(out actionResponse))
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
                var response = await ActionResponse<List<PlanDayDto>>.ReturnSuccess(null, "Studenti uspješno dodani u grupu.");
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

        public async Task<ActionResponse<PlanDayDto>> AddDayToPlan(PlanDayDto planDay)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDayDto, PlanDay>(planDay);
                unitOfWork.GetGenericRepository<PlanDay>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<PlanDayDto>
                    .ReturnSuccess(mapper.Map<PlanDay, PlanDayDto>(entityToAdd), "Dan uspješno dodan u plan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, planDay);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }
    }
}
