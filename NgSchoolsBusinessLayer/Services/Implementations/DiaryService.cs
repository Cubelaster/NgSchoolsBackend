using AutoMapper;
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
    public class DiaryService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public DiaryService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<DiaryDto>> Insert(DiaryDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<DiaryDto, Diary>(entityDto);
                unitOfWork.GetGenericRepository<Diary>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                //if ((await ModifySubjectThemes(entityDto)).IsNotSuccess(out ActionResponse<SubjectDto> response, out SubjectDto subjectDto))
                //{
                //    return response;
                //}

                return await ActionResponse<DiaryDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom upisa dnevnika rada.");
            }
        }

        //public async Task<ActionResponse<List<DiaryDto>>> ModifySubjectsForEducationProgram(List<SubjectDto> subjects)
        //{
        //    try
        //    {
        //        var response = await ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Predmeti uspješno ažurirani.");

        //        var existingSubjects = mapper.Map<List<SubjectDto>>(unitOfWork.GetGenericRepository<Subject>()
        //            .GetAll(s => s.EducationProgramId ==
        //                    subjects.Select(su => su.EducationProgramId.Value).FirstOrDefault()
        //            ));

        //        var subjectsToUpdate = subjects.Where(s => existingSubjects.Select(ss => ss.Id).Contains(s.Id)).ToList();
        //        var subjectsToInsert = subjects.Where(s => !existingSubjects.Select(ss => ss.Id).Contains(s.Id)).ToList();
        //        var subjectsToDelete = existingSubjects.Where(es => !subjects.Select(ss => ss.Id).Contains(es.Id)).ToList();

        //        if ((await InsertSubjects(subjectsToInsert)).IsNotSuccess(out response, out subjects))
        //        {
        //            return response;
        //        }

        //        if ((await UpdateSubjects(subjectsToUpdate)).IsNotSuccess(out response, out subjects))
        //        {
        //            return response;
        //        }

        //        if ((await DeleteSubjects(subjectsToDelete)).IsNotSuccess(out response, out subjects))
        //        {
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, subjects);
        //        return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom upisa studenata.");
        //    }
        //}

        //    public async Task<ActionResponse<List<SubjectDto>>> InsertSubjects(List<SubjectDto> subjects)
        //    {
        //        try
        //        {
        //            var response = await ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Unos predmeta uspješan.");
        //            subjects.ForEach(async s =>
        //            {
        //                if ((await Insert(s)).IsNotSuccess(out ActionResponse<SubjectDto> insertResponse, out s))
        //                {
        //                    response = await ActionResponse<List<SubjectDto>>.ReturnError(insertResponse.Message);
        //                    return;
        //                }
        //            });

        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<List<SubjectDto>>.ReturnError("Greška prilikom upisa studenta.");
        //        }
        //    }

        //    public async Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto)
        //    {
        //        try
        //        {
        //            var entityToUpdate = unitOfWork.GetGenericRepository<Subject>().FindSingle(entityDto.Id.Value);
        //            mapper.Map(entityDto, entityToUpdate);
        //            unitOfWork.GetGenericRepository<Subject>().Update(entityToUpdate);
        //            unitOfWork.Save();

        //            if ((await ModifySubjectThemes(entityDto)).IsNotSuccess(out ActionResponse<SubjectDto> response, out SubjectDto subjectDto))
        //            {
        //                return response;
        //            }

        //            return await ActionResponse<SubjectDto>.ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<SubjectDto>.ReturnError($"Greška prilikom ažuriranja predmeta: {entityDto.Name}.");
        //        }
        //    }

        //    public async Task<ActionResponse<List<SubjectDto>>> UpdateSubjects(List<SubjectDto> subjects)
        //    {
        //        try
        //        {
        //            var response = ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Ažuriranje predmeta uspješno.");

        //            subjects.ForEach(async s =>
        //            {
        //                if ((await Update(s)).IsNotSuccess(out ActionResponse<SubjectDto> updateResponse, out s))
        //                {
        //                    response = ActionResponse<List<SubjectDto>>.ReturnError(updateResponse.Message);
        //                    return;
        //                }
        //            });

        //            return await response;
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<List<SubjectDto>>.ReturnError($"Greška prilikom ažuriranja predmeta.");
        //        }
        //    }

        //    public async Task<ActionResponse<SubjectDto>> Delete(int id)
        //    {
        //        try
        //        {
        //            unitOfWork.GetGenericRepository<Subject>().Delete(id);
        //            unitOfWork.Save();
        //            return await ActionResponse<SubjectDto>.ReturnSuccess(null, "Brisanje uspješno.");
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<SubjectDto>.ReturnError("Greška prilikom brisanja predmeta.");
        //        }
        //    }

        //    public async Task<ActionResponse<List<SubjectDto>>> DeleteSubjects(List<SubjectDto> subjects)
        //    {
        //        try
        //        {
        //            var response = ActionResponse<List<SubjectDto>>.ReturnSuccess(subjects, "Brisanje predmeta uspješno.");

        //            subjects.ForEach(async s =>
        //            {
        //                if ((await Delete(s.Id.Value)).IsNotSuccess(out ActionResponse<SubjectDto> deleteResponse, out s))
        //                {
        //                    response = ActionResponse<List<SubjectDto>>.ReturnError(deleteResponse.Message);
        //                    return;
        //                }
        //            });

        //            return await response;
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<List<SubjectDto>>.ReturnError($"Greška prilikom brisanja predmeta.");
        //        }
        //    }

        //    public async Task<ActionResponse<SubjectDto>> ModifySubjectThemes(SubjectDto entityDto)
        //    {
        //        try
        //        {
        //            var response = await ActionResponse<SubjectDto>.ReturnSuccess(entityDto, "Predmetne teme uspješno ažurirane.");

        //            var currentThemes = mapper.Map<List<SubjectTheme>, List<SubjectThemeDto>>(
        //                unitOfWork.GetGenericRepository<Subject>()
        //                .FindBy(s => s.Id == entityDto.Id, includeProperties: "SubjectThemes.Theme")
        //                .SubjectThemes.ToList());

        //            var themesToAdd = entityDto.Themes
        //                .Where(t => !currentThemes.Select(ct => ct.ThemeId).Contains(t.Id))
        //                .Select(st => new SubjectThemeDto
        //                {
        //                    SubjectId = entityDto.Id,
        //                    ThemeId = st.Id
        //                })
        //                .ToList();

        //            var themesToRemove = currentThemes
        //                .Where(ct => !entityDto.Themes.Select(et => et.Id).Contains(ct.ThemeId))
        //                .Select(st => new SubjectThemeDto
        //                {
        //                    Id = st.Id,
        //                    ThemeId = st.ThemeId,
        //                    SubjectId = st.SubjectId
        //                })
        //                .ToList();


        //            if ((await AddThemesToSubject(themesToAdd)).IsNotSuccess(out ActionResponse<List<SubjectThemeDto>> subjectThemesResponse, out themesToAdd))
        //            {
        //                return response;
        //            }

        //            if ((await RemoveThemesFromSubject(themesToRemove)).IsNotSuccess(out subjectThemesResponse, out themesToRemove))
        //            {
        //                return response;
        //            }

        //            return response;

        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<SubjectDto>.ReturnError($"Greška prilikom mijenjanja predmetnih tema.");
        //        }
        //    }

        //    public async Task<ActionResponse<SubjectThemeDto>> AddThemeToSubject(SubjectThemeDto entityDto)
        //    {
        //        try
        //        {
        //            var entityToAdd = mapper.Map<SubjectThemeDto, SubjectTheme>(entityDto);
        //            unitOfWork.GetGenericRepository<SubjectTheme>().Add(entityToAdd);
        //            unitOfWork.Save();
        //            mapper.Map(entityToAdd, entityDto);
        //            return await ActionResponse<SubjectThemeDto>.ReturnSuccess(entityDto);
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<SubjectThemeDto>.ReturnError("Greška prilikom dodavanja teme predmetu.");
        //        }
        //    }

        //    public async Task<ActionResponse<List<SubjectThemeDto>>> AddThemesToSubject(List<SubjectThemeDto> subjectThemes)
        //    {
        //        try
        //        {
        //            var response = await ActionResponse<List<SubjectThemeDto>>.ReturnSuccess(subjectThemes, "Dodavanje tema predmetu uspješno.");
        //            subjectThemes.ForEach(async st =>
        //            {
        //                if ((await AddThemeToSubject(st)).IsNotSuccess(out ActionResponse<SubjectThemeDto> insertResponse, out st))
        //                {
        //                    response = await ActionResponse<List<SubjectThemeDto>>.ReturnError(insertResponse.Message);
        //                    return;
        //                }
        //            });

        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<List<SubjectThemeDto>>.ReturnError("Greška prilikom dodavanja tema predmetu.");
        //        }
        //    }

        //    public async Task<ActionResponse<List<SubjectThemeDto>>> RemoveThemesFromSubject(List<SubjectThemeDto> subjectThemes)
        //    {
        //        try
        //        {
        //            var response = await ActionResponse<List<SubjectThemeDto>>.ReturnSuccess(subjectThemes, "Micanje tema iz predmeta usjpešno.");
        //            subjectThemes.ForEach(async st =>
        //            {
        //                if ((await RemoveThemeFromSubject(st)).IsNotSuccess(out ActionResponse<SubjectThemeDto> insertResponse, out st))
        //                {
        //                    response = await ActionResponse<List<SubjectThemeDto>>.ReturnError(insertResponse.Message);
        //                    return;
        //                }
        //            });

        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<List<SubjectThemeDto>>.ReturnError("Greška prilikom dodavanja tema predmetu.");
        //        }
        //    }

        //    public async Task<ActionResponse<SubjectThemeDto>> RemoveThemeFromSubject(SubjectThemeDto subjectTheme)
        //    {
        //        try
        //        {
        //            unitOfWork.GetGenericRepository<SubjectTheme>().Delete(subjectTheme.Id.Value);
        //            unitOfWork.Save();
        //            return await ActionResponse<SubjectThemeDto>.ReturnSuccess(null, "Brisanje uspješno.");
        //        }
        //        catch (Exception ex)
        //        {
        //            loggerService.LogErrorToEventLog(ex);
        //            return await ActionResponse<SubjectThemeDto>.ReturnError("Greška prilikom brisanja teme iz predmeta.");
        //        }
        //    }
        //}
    }
}
