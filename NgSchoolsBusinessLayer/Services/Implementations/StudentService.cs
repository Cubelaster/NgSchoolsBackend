using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class StudentService : IStudentService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public StudentService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<StudentDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Student>().FindBy(c => c.Id == id, includeProperties: "Photo,Files.File");
                return await ActionResponse<StudentDto>
                    .ReturnSuccess(mapper.Map<Student, StudentDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom dohvata studenta.");
            }
        }

        public async Task<ActionResponse<StudentDto>> GetByOib(string oib)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Student>().FindBy(c => c.Oib == oib, includeProperties: "Photo,Files.File");
                return await ActionResponse<StudentDto>
                    .ReturnSuccess(mapper.Map<Student, StudentDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom dohvata studenta.");
            }
        }

        [CacheRefreshSource(typeof(StudentDto))]
        public async Task<ActionResponse<List<StudentDto>>> GetAllForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<Student>().GetAll(includeProperties: "Photo,Files.File");
                return await ActionResponse<List<StudentDto>>.ReturnSuccess(
                    mapper.Map<List<Student>, List<StudentDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<StudentDto>>.ReturnError("Greška prilikom dohvata svih studenata.");
            }
        }

        public async Task<ActionResponse<List<StudentDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Student>().GetAll(includeProperties: "Photo,Files.File");
                return await ActionResponse<List<StudentDto>>
                    .ReturnSuccess(mapper.Map<List<Student>, List<StudentDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<StudentDto>>.ReturnError("Greška prilikom dohvata svih studenata.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<StudentDto> students = new List<StudentDto>();
                var cachedResponse = await cacheService.GetFromCache<List<StudentDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out students))
                {
                    students = (await GetAll()).GetData();
                }

                var pagedResult = await students.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<StudentDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<StudentDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za studente.");
            }
        }

        public async Task<ActionResponse<StudentDto>> Insert(StudentDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentDto, Student>(entityDto);
                unitOfWork.GetGenericRepository<Student>().Add(entityToAdd);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(entityToAdd).Reference(p => p.Photo).Load();
                await cacheService.RefreshCache<List<StudentDto>>();
                return await ActionResponse<StudentDto>
                    .ReturnSuccess(mapper.Map<Student, StudentDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom upisa studenta.");
            }
        }

        public async Task<ActionResponse<StudentDto>> Update(StudentDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<StudentDto, Student>(entityDto);
                unitOfWork.GetGenericRepository<Student>().Update(entityToUpdate);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(entityToUpdate).Reference(p => p.Photo).Load();
                await cacheService.RefreshCache<List<StudentDto>>();
                return await ActionResponse<StudentDto>
                    .ReturnSuccess(mapper.Map<Student, StudentDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<StudentDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Student>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<StudentDto>>();
                return await ActionResponse<StudentDto>.ReturnSuccess(null, "Brisanje studenta uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom brisanja studenta.");
            }
        }
    }
}
