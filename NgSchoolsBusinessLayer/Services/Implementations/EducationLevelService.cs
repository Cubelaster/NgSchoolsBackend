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
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class EducationLevelService : IEducationLevelService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public EducationLevelService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<EducationLevelDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<EducationLevel>().FindBy(c => c.Id == id);
                return await ActionResponse<EducationLevelDto>
                    .ReturnSuccess(mapper.Map<EducationLevel, EducationLevelDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationLevelDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<EducationLevelDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<EducationLevel>().GetAll();
                return await ActionResponse<List<EducationLevelDto>>
                    .ReturnSuccess(mapper.Map<List<EducationLevel>, List<EducationLevelDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<EducationLevelDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<EducationLevelDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<EducationLevel>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<EducationLevelDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<EducationLevel>, List<EducationLevelDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<EducationLevelDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<EducationLevelDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<EducationLevelDto>> Insert(EducationLevelDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<EducationLevelDto, EducationLevel>(entityDto);
                unitOfWork.GetGenericRepository<EducationLevel>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<EducationLevelDto>
                    .ReturnSuccess(mapper.Map<EducationLevel, EducationLevelDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationLevelDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<EducationLevelDto>> Update(EducationLevelDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<EducationLevelDto, EducationLevel>(entityDto);
                unitOfWork.GetGenericRepository<EducationLevel>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<EducationLevelDto>
                    .ReturnSuccess(mapper.Map<EducationLevel, EducationLevelDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationLevelDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
