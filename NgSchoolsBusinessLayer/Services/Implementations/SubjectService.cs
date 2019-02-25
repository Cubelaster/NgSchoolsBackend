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
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public SubjectService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        #region Readers

        public async Task<ActionResponse<SubjectDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Subject>()
                    .FindBy(c => c.Id == id, includeProperties: "Themes");
                return await ActionResponse<SubjectDto>
                    .ReturnSuccess(mapper.Map<Subject, SubjectDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<SubjectDto>.ReturnError("Greška prilikom dohvata predmeta.");
            }
        }

        public async Task<ActionResponse<List<SubjectDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Subject>()
                    .GetAll(includeProperties: "Themes");
                return await ActionResponse<List<SubjectDto>>
                    .ReturnSuccess(mapper.Map<List<Subject>, List<SubjectDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom dohvata svih predmeta.");
            }
        }

        public async Task<ActionResponse<List<SubjectDto>>> GetAllByEducationProgramId(int educationProgramId)
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Subject>()
                    .GetAll(s => s.EducationProgramId == educationProgramId, includeProperties: "Themes");
                return await ActionResponse<List<SubjectDto>>
                    .ReturnSuccess(mapper.Map<List<Subject>, List<SubjectDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom dohvata svih predmeta za program.");
            }
        }

        public async Task<ActionResponse<PagedResult<SubjectDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<Subject>()
                    .GetAllAsQueryable(includeProperties: "Themes").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<SubjectDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<Subject>, List<SubjectDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<SubjectDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<SubjectDto>>.ReturnError("Greška prilikom dohvata straničnih podataka predmeta.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<Subject>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja predmeta.");
            }
        }

        #endregion Readers

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
            catch (Exception)
            {
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
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom upisa studenta.");
            }
        }

        public async Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<Subject>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<Subject>().Update(entityToUpdate);
                unitOfWork.Save();

                return await ActionResponse<SubjectDto>.ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<List<SubjectDto>>.ReturnError($"Greška prilikom brisanja predmeta.");
            }
        }
    }
}
