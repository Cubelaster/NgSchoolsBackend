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
        private readonly ISubjectService subjectService;
        private readonly IThemeService themeService;

        public EducationProgramService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, IPlanService planService, ISubjectService subjectService,
            IThemeService themeService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.planService = planService;
            this.subjectService = subjectService;
            this.themeService = themeService;
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

        public async Task<ActionResponse<EducationProgramDto>> InsertCompleteProgram(EducationProgramDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if ((await Insert(entityDto)).IsNotSuccess(out ActionResponse<EducationProgramDto> response, out entityDto))
                    {
                        return response;
                    }

                    var themesInProgram = entityDto.Subjects
                        .SelectMany(s => s.Themes)
                        .GroupBy(t => t.Name)
                        .Select(g => g.FirstOrDefault())
                        .Where(t => t != null)
                        .ToList();

                    if ((await themeService.ModifyThemesForEducationProgram(themesInProgram))
                        .IsNotSuccess(out ActionResponse<List<ThemeDto>> themesResponse, out themesInProgram))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(themesResponse.Message);
                    }

                    var subjectsInProgram = entityDto.Subjects
                        .Select(s => {
                            s.EducationProgramId = entityDto.Id;
                            s.Themes.ForEach(t => t.Id = themesInProgram
                                .FirstOrDefault(tip => tip.Name == t.Name).Id);
                            return s;
                        })
                        .ToList();

                    if ((await subjectService.ModifySubjectsForEducationProgram(subjectsInProgram)).IsNotSuccess(out ActionResponse<List<SubjectDto>> subjectResponse, out List<SubjectDto> modifiedSubjects))
                    {
                        return await ActionResponse<EducationProgramDto>.ReturnError(subjectResponse.Message);
                    }

                    entityDto.Plan.EducationPogramId = entityDto.Id;
                    if ((await planService.InsertPlanForEducationProgram(entityDto)).IsNotSuccess(out ActionResponse<EducationProgramDto> planResponse, out entityDto))
                    {
                        return planResponse;
                    }

                    scope.Complete();
                    return await ActionResponse<EducationProgramDto>
                        .ReturnSuccess(entityDto);
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom upisivanja programa.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Insert(EducationProgramDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom ažuriranja programa.");
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
                var planEntity = unitOfWork.GetGenericRepository<Plan>().FindBy(p => p.EducationPogramId == id);
                unitOfWork.GetGenericRepository<Plan>().Delete(planEntity.Id);
                unitOfWork.GetGenericRepository<EducationProgram>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>.ReturnSuccess(null, "Brisanje programa i povezanog plana uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
