using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PlanDayService : IPlanDayService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public PlanDayService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<List<PlanDayDto>>> InsertPlanDays(List<PlanDayDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<PlanDayDto>>.ReturnSuccess(entityDtos, "Dani plana uspješno upisani");
                entityDtos.ForEach(async pd =>
                {
                    if ((await Insert(pd)).IsNotSuccess(out ActionResponse<PlanDayDto> insertResponse, out pd))
                    {
                        response = await ActionResponse<List<PlanDayDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<PlanDayDto>>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }

        public async Task<ActionResponse<PlanDayDto>> Insert(PlanDayDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<PlanDayDto, PlanDay>(entityDto);
                unitOfWork.GetGenericRepository<PlanDay>().Add(entityToAdd);
                mapper.Map(entityToAdd, entityDto);

                return await ActionResponse<PlanDayDto>.ReturnSuccess(entityDto, "Dan plana uspješno upisan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDayDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }
    }
}
