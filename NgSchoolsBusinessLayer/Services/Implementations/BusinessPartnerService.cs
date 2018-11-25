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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public BusinessPartnerService(IMapper mapper, ILoggerService loggerService, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<BusinessPartnerDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<BusinessPartner>().FindBy(c => c.Id == id);
                return await ActionResponse<BusinessPartnerDto>
                    .ReturnSuccess(mapper.Map<BusinessPartner, BusinessPartnerDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<BusinessPartnerDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<BusinessPartner>().GetAll();
                return await ActionResponse<List<BusinessPartnerDto>>
                    .ReturnSuccess(mapper.Map<List<BusinessPartner>, List<BusinessPartnerDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<BusinessPartnerDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<BusinessPartnerDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<BusinessPartner>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<BusinessPartnerDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<BusinessPartner>, List<BusinessPartnerDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<BusinessPartnerDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<BusinessPartnerDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Insert(BusinessPartnerDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<BusinessPartnerDto, BusinessPartner>(entityDto);
                unitOfWork.GetGenericRepository<BusinessPartner>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<BusinessPartnerDto>
                    .ReturnSuccess(mapper.Map<BusinessPartner, BusinessPartnerDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Update(BusinessPartnerDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<BusinessPartnerDto, BusinessPartner>(entityDto);
                unitOfWork.GetGenericRepository<BusinessPartner>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<BusinessPartnerDto>
                    .ReturnSuccess(mapper.Map<BusinessPartner, BusinessPartnerDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<BusinessPartner>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<BusinessPartnerDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
