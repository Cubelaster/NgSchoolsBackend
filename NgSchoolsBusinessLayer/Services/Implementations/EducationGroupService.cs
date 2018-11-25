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
    public class EducationGroupService : IEducationGroupService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public EducationGroupService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<EducationGroupDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<EducationGroups>().FindBy(c => c.Id == id);
                return await ActionResponse<EducationGroupDto>
                    .ReturnSuccess(mapper.Map<EducationGroups, EducationGroupDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationGroupDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<EducationGroupDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<EducationGroups>().GetAll();
                return await ActionResponse<List<EducationGroupDto>>
                    .ReturnSuccess(mapper.Map<List<EducationGroups>, List<EducationGroupDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<EducationGroupDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<EducationGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<EducationGroups>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<EducationGroupDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<EducationGroups>, List<EducationGroupDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<EducationGroupDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<EducationGroupDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<EducationGroupDto>> Insert(EducationGroupDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<EducationGroupDto, EducationGroups>(entityDto);
                unitOfWork.GetGenericRepository<EducationGroups>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<EducationGroupDto>
                    .ReturnSuccess(mapper.Map<EducationGroups, EducationGroupDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationGroupDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<EducationGroupDto>> Update(EducationGroupDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<EducationGroupDto, EducationGroups>(entityDto);
                unitOfWork.GetGenericRepository<EducationGroups>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<EducationGroupDto>
                    .ReturnSuccess(mapper.Map<EducationGroups, EducationGroupDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationGroupDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<EducationGroupDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<EducationGroups>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<EducationGroupDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationGroupDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
