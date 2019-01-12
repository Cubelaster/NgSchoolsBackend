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
using System.Transactions;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class EducationProgramService : IEducationProgramService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlanService planService;

        public EducationProgramService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, IPlanService planService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.planService = planService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<EducationProgramDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<EducationProgram>()
                    .FindBy(c => c.Id == id);
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map<EducationProgram, EducationProgramDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom dohvaćanja programa.");
            }
        }

        public async Task<ActionResponse<List<EducationProgramDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAll();
                return await ActionResponse<List<EducationProgramDto>>
                    .ReturnSuccess(mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<EducationProgramDto>>.ReturnError("Greška prilikom dohvaćanja svih programa.");
            }
        }

        public async Task<ActionResponse<PagedResult<EducationProgramDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<EducationProgramDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za programe.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Insert(EducationProgramDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var planEntity = mapper.Map<PlanDto, Plan>(entityDto.Plan);

                    var entityToAdd = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                    unitOfWork.GetGenericRepository<EducationProgram>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);

                    return await ActionResponse<EducationProgramDto>
                        .ReturnSuccess(mapper.Map<EducationProgram, EducationProgramDto>(entityToAdd));
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom upisivanja programa.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Update(EducationProgramDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map<EducationProgram, EducationProgramDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom ažuriranja programa.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<EducationProgram>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>.ReturnSuccess(null, "Brisanje programa uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
