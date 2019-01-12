using AutoMapper;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
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

        public async Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto)
        {
            try
            {
                var planToAdd = mapper.Map<PlanDto, Plan>(entityDto);
                unitOfWork.GetGenericRepository<Plan>().Add(planToAdd);
                mapper.Map(planToAdd, entityDto);

                return await ActionResponse<PlanDto>.ReturnSuccess(entityDto, "Plan uspješno upisan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }
    }
}
