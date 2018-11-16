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
    public class ClassTypeService : IClassTypeService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public ClassTypeService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ClassTypeDto>> GetById(int id)
        {
            try
            {
                var classType = unitOfWork.GetGenericRepository<ClassType>().FindBy(c => c.Id == id);
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(classType));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<ClassTypeDto>>> GetAll()
        {
            try
            {
                var classTypes = unitOfWork.GetGenericRepository<ClassType>().GetAll();
                return await ActionResponse<List<ClassTypeDto>>
                    .ReturnSuccess(mapper.Map<List<ClassType>, List<ClassTypeDto>>(classTypes));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ClassTypeDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<ClassTypeDto>>> GetAllClassTypesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<ClassTypeDto> classTypes = new List<ClassTypeDto>();

                var bla = unitOfWork.GetGenericRepository<ClassType>()
                    .GetAllAsQueryable();

                var pagedEntityResult = await unitOfWork.GetGenericRepository<ClassType>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<ClassTypeDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<ClassType>, List<ClassTypeDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<ClassTypeDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ClassTypeDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<ClassTypeDto>> Insert(ClassTypeDto classType)
        {
            try
            {
                var classTypeToAdd = mapper.Map<ClassTypeDto, ClassType>(classType);
                unitOfWork.GetGenericRepository<ClassType>().Add(classTypeToAdd);
                unitOfWork.Save();
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(classTypeToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<ClassTypeDto>> Update(ClassTypeDto classType)
        {
            try
            {
                var classTypeToUpdate = mapper.Map<ClassTypeDto, ClassType>(classType);
                unitOfWork.GetGenericRepository<ClassType>().Update(classTypeToUpdate);
                unitOfWork.Save();
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(classTypeToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
