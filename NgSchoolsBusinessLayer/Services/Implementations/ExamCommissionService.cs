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
    public class ExamCommissionService : IExamCommissionService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public ExamCommissionService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ExamCommissionDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ExamCommission>().FindBy(c => c.Id == id);
                return await ActionResponse<ExamCommissionDto>
                    .ReturnSuccess(mapper.Map<ExamCommission, ExamCommissionDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ExamCommissionDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<ExamCommissionDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<ExamCommission>()
                    .GetAll(includeProperties: "UserExamCommissions.User");
                return await ActionResponse<List<ExamCommissionDto>>
                    .ReturnSuccess(mapper.Map<List<ExamCommission>, List<ExamCommissionDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ExamCommissionDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<ExamCommissionDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<ExamCommission>()
                    .GetAllAsQueryable(includeProperties: "UserExamCommissions.User").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<ExamCommissionDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<ExamCommission>, List<ExamCommissionDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<ExamCommissionDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ExamCommissionDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Insert(ExamCommissionDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<ExamCommissionDto, ExamCommission>(entityDto);
                unitOfWork.GetGenericRepository<ExamCommission>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<ExamCommissionDto>
                    .ReturnSuccess(mapper.Map<ExamCommission, ExamCommissionDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ExamCommissionDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Update(ExamCommissionDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<ExamCommissionDto, ExamCommission>(entityDto);
                unitOfWork.GetGenericRepository<ExamCommission>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<ExamCommissionDto>
                    .ReturnSuccess(mapper.Map<ExamCommission, ExamCommissionDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ExamCommissionDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<ExamCommission>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ExamCommissionDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
