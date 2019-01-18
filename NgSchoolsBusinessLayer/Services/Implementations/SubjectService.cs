using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var response = await ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Predmeti uspješno ažurirani.");

                var existingSubjects = mapper.Map<List<SubjectDto>>(unitOfWork.GetGenericRepository<Subject>()
                    .GetAll(s => s.EducationProgramId ==
                            subjects.Select(su => su.EducationProgramId.Value).FirstOrDefault()
                    ));

                var subjectsToUpdate = subjects.Where(s => existingSubjects.Select(ss => ss.Id).Contains(s.Id)).ToList();
                var subjectsToInsert = subjects.Where(s => !existingSubjects.Select(ss => ss.Id).Contains(s.Id)).ToList();
                var subjectsToDelete = existingSubjects.Where(es => !subjects.Select(ss => ss.Id).Contains(es.Id)).ToList();

                if ((await InsertSubjects(subjectsToInsert)).IsNotSuccess(out response, out subjects))
                {
                    return response;
                }

                if ((await UpdateSubjects(subjectsToUpdate)).IsNotSuccess(out response, out subjects))
                {
                    return response;
                }

                if ((await DeleteSubjects(subjectsToDelete)).IsNotSuccess(out response, out subjects))
                {
                    return response;
                }

                return response;
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
                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<SubjectDto>
                    .ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<SubjectDto>.ReturnError("Greška prilikom upisa studenta.");
            }
        }

        public async Task<ActionResponse<List<SubjectDto>>> InsertSubjects(List<SubjectDto> subjects)
        {
            try
            {
                var response = await ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Unos predmeta uspješan.");
                subjects.ForEach(async s =>
                {
                    if ((await Insert(s)).IsNotSuccess(out ActionResponse<SubjectDto> insertResponse, out s))
                    {
                        response = await ActionResponse<List<SubjectDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom upisa studenta.");
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

        public async Task<ActionResponse<List<SubjectDto>>> UpdateSubjects(List<SubjectDto> subjects)
        {
            try
            {
                var response = ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Ažuriranje predmeta uspješno.");

                subjects.ForEach(async s =>
                {
                    if ((await Update(s)).IsNotSuccess(out ActionResponse<SubjectDto> updateResponse, out s))
                    {
                        response = ActionResponse<List<SubjectDto>>.ReturnError(updateResponse.Message);
                        return;
                    }
                });

                return await response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<SubjectDto>>.ReturnError($"Greška prilikom ažuriranja predmeta.");
            }
        }

        public async Task<ActionResponse<SubjectDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Subject>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<SubjectDto>.ReturnSuccess(null, "Brisanje uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<SubjectDto>.ReturnError("Greška prilikom brisanja predmeta.");
            }
        }

        public async Task<ActionResponse<List<SubjectDto>>> DeleteSubjects(List<SubjectDto> subjects)
        {
            try
            {
                var response = ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Brisanje predmeta uspješno.");

                subjects.ForEach(async s =>
                {
                    if ((await Delete(s.Id.Value)).IsNotSuccess(out ActionResponse<SubjectDto> deleteResponse, out s))
                    {
                        response = ActionResponse<List<SubjectDto>>.ReturnError(deleteResponse.Message);
                        return;
                    }
                });

                return await response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<SubjectDto>>.ReturnError($"Greška prilikom brisanja predmeta.");
            }
        }
    }
}
