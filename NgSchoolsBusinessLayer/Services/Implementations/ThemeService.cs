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

        public async Task<ActionResponse<List<ThemeDto>>> ModifyThemesForEducationProgram(List<ThemeDto> entityDtos)
        {
            try
            {
                var response = ActionResponse<List<ThemeDto>>.ReturnSuccess(entityDtos, "Teme uspješno ažurirane.");
                entityDtos.ForEach(async s =>
                {
                    if (s.Id.HasValue)
                    {
                        if ((await Update(s)).IsNotSuccess(out ActionResponse<ThemeDto> updateResponse, out s))
                        {
                            response = ActionResponse<List<ThemeDto>>.ReturnError(updateResponse.Message);
                            return;
                        }
                    }
                    else
                    {
                        if ((await Insert(s)).IsNotSuccess(out ActionResponse<ThemeDto> insertResponse, out s))
                        {
                            response = ActionResponse<List<ThemeDto>>.ReturnError(insertResponse.Message);
                            return;
                        }
                    }
                });

                return await response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<ThemeDto>>.ReturnError("Greška prilikom upisa tema.");
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
    }
}
