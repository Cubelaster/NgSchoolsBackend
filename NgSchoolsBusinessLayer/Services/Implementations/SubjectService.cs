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
    public class SubjectService : ISubjectService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public SubjectService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<List<SubjectDto>>> ModifySubjectsForEducationProgram(List<SubjectDto> subjects)
        {
            try
            {
                var response = ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Predmeti uspješno ažurirani.");
                subjects.ForEach(async s =>
                {
                    if (s.Id.HasValue)
                    {
                        if ((await Update(s)).IsNotSuccess(out ActionResponse<SubjectDto> updateResponse, out s))
                        {
                            response = ActionResponse<List<SubjectDto>>.ReturnError(updateResponse.Message);
                            return;
                        }
                    }
                    else
                    {
                        if ((await Insert(s)).IsNotSuccess(out ActionResponse<SubjectDto> insertResponse, out s))
                        {
                            response = ActionResponse<List<SubjectDto>>.ReturnError(insertResponse.Message);
                            return;
                        }
                    }
                });

                return await response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, subjects);
                return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom upisa studenata.");
            }
        }

        public async Task<ActionResponse<SubjectDto>> Insert(SubjectDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<SubjectDto, Subject>(entityDto);
                unitOfWork.GetGenericRepository<Subject>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<SubjectDto>
                    .ReturnSuccess(mapper.Map<Subject, SubjectDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<SubjectDto>.ReturnError("Greška prilikom upisa studenta.");
            }
        }

        public async Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<SubjectDto, Subject>(entityDto);
                unitOfWork.GetGenericRepository<Subject>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<SubjectDto>
                    .ReturnSuccess(mapper.Map<Subject, SubjectDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<SubjectDto>.ReturnError($"Greška prilikom ažuriranja predmeta: {entityDto.Name}.");
            }
        }

    }
}
