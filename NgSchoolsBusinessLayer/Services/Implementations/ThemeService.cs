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
    public class ThemeService : IThemeService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public ThemeService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        #region Readers

        public async Task<ActionResponse<ThemeDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Theme>()
                    .FindBy(c => c.Id == id);
                return await ActionResponse<ThemeDto>
                    .ReturnSuccess(mapper.Map<Theme, ThemeDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ThemeDto>.ReturnError("Greška prilikom dohvata teme.");
            }
        }

        public async Task<ActionResponse<List<ThemeDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Theme>().GetAll();
                return await ActionResponse<List<ThemeDto>>
                    .ReturnSuccess(mapper.Map<List<Theme>, List<ThemeDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ThemeDto>>.ReturnError("Greška prilikom dohvata svih tema.");
            }
        }

        public async Task<ActionResponse<List<ThemeDto>>> GetAllBySubjectId(int subjectId)
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Theme>()
                    .GetAll(s => s.SubjectId == subjectId);
                return await ActionResponse<List<ThemeDto>>
                    .ReturnSuccess(mapper.Map<List<Theme>, List<ThemeDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ThemeDto>>.ReturnError("Greška prilikom dohvata svih tema za predmet.");
            }
        }

        public async Task<ActionResponse<PagedResult<ThemeDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<Theme>()
                    .GetAllAsQueryable().GetPaged(pagedRequest);

                var pagedResult = new PagedResult<ThemeDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<Theme>, List<ThemeDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<ThemeDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ThemeDto>>.ReturnError("Greška prilikom dohvata straničnih podataka tema.");
            }
        }

        #endregion Readers

        public async Task<ActionResponse<ThemeDto>> Insert(ThemeDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<ThemeDto, Theme>(entityDto);
                unitOfWork.GetGenericRepository<Theme>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<ThemeDto>.ReturnSuccess(entityDto, "Tema uspješno upisana.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<ThemeDto>.ReturnError("Greška prilikom upisivanja teme.");
            }
        }

        public async Task<ActionResponse<ThemeDto>> Update(ThemeDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<ThemeDto, Theme>(entityDto);
                unitOfWork.GetGenericRepository<Theme>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<ThemeDto>
                    .ReturnSuccess(mapper.Map<Theme, ThemeDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ThemeDto>.ReturnError($"Greška prilikom ažuriranja teme: {entityDto.Name}.");
            }
        }

        public async Task<ActionResponse<ThemeDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Theme>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<ThemeDto>.ReturnSuccess(null, "Brisanje uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ThemeDto>.ReturnError("Greška prilikom brisanja teme.");
            }
        }
    }
}
