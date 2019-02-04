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
                var entity = unitOfWork.GetGenericRepository<Student>()
                    .FindBy(c => c.Id == id, 
                    includeProperties: "Photo,Files.File,AddressCity,AddressCountry,AddressRegion,EmployerCountry,EmployerCity,EmployerRegion,CountryOfBirth,RegionOfBirth,CityOfBirth");
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
                var entity = unitOfWork.GetGenericRepository<Student>()
                    .FindBy(c => c.Oib == oib,
                    includeProperties: "Photo,Files.File,AddressCity,AddressCountry,AddressRegion,EmployerCountry,EmployerCity,EmployerRegion,CountryOfBirth,RegionOfBirth,CityOfBirth");
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
                var allEntities = unitOfWork.GetGenericRepository<Student>()
                    .GetAll(includeProperties: "Photo,Files.File,AddressCity,AddressCountry,AddressRegion,EmployerCountry,EmployerCity,EmployerRegion,CountryOfBirth,RegionOfBirth,CityOfBirth");
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
                var allEntities = unitOfWork.GetGenericRepository<Student>()
                    .GetAll(includeProperties: "Photo,Files.File,AddressCity,AddressCountry,AddressRegion,EmployerCountry,EmployerCity,EmployerRegion,CountryOfBirth,RegionOfBirth,CityOfBirth");
                return await ActionResponse<List<StudentDto>>
                    .ReturnSuccess(mapper.Map<List<Student>, List<StudentDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<StudentDto>>.ReturnError("Greška prilikom dohvata svih studenata.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentDto>>> GetBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<StudentDto> students = new List<StudentDto>();
                var cachedResponse = await cacheService.GetFromCache<List<StudentDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out students))
                {
                    students = (await GetAll()).GetData();
                }

                var pagedResult = await students.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<StudentDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<StudentDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za studente.");
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
                List<FileDto> files = entityDto.Files != null ? new List<FileDto>(entityDto.Files) : new List<FileDto>();
                entityDto.Files = null;

                var entityToAdd = mapper.Map<StudentDto, Student>(entityDto);
                unitOfWork.GetGenericRepository<Student>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);
                entityDto.Files = files;
                if ((await ModifyStudentFiles(entityDto))
                    .IsNotSuccess(out ActionResponse<StudentDto> fileResponse, out entityDto))
                {
                    return fileResponse;
                }

                await cacheService.RefreshCache<List<StudentDto>>();

                if ((await cacheService.GetFromCache<List<StudentDto>>())
                    .IsNotSuccess(out ActionResponse<List<StudentDto>> cacheResponse,
                        out List<StudentDto> students))
                {
                    return await ActionResponse<StudentDto>.ReturnError("Greška prilikom povrata podataka studenta.");
                }

                return await ActionResponse<StudentDto>.ReturnSuccess(students.FirstOrDefault(s => s.Id == entityDto.Id));
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
                List<FileDto> files = new List<FileDto>(entityDto.Files);
                entityDto.Files = null;

                var entityToUpdate = unitOfWork.GetGenericRepository<Student>()
                    .FindBy(c => c.Id == entityDto.Id, 
                    includeProperties: "Photo,Files.File,AddressCity,AddressCountry,AddressRegion,EmployerCountry,EmployerCity,EmployerRegion");

                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<Student>().Update(entityToUpdate);
                unitOfWork.Save();

                mapper.Map(entityToUpdate, entityDto);
                entityDto.Files = files;
                if ((await ModifyStudentFiles(entityDto))
                    .IsNotSuccess(out ActionResponse<StudentDto> fileResponse, out entityDto))
                {
                    return fileResponse;
                }

                await cacheService.RefreshCache<List<StudentDto>>();
                return await ActionResponse<StudentDto>
                    .ReturnSuccess(mapper.Map<Student, StudentDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom ažuriranja podataka za studenta.");
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

        public async Task<ActionResponse<StudentDto>> ModifyStudentFiles(StudentDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Student>()
                    .FindBy(e => e.Id == entityDto.Id, includeProperties: "Files.File");

                var currentFiles = mapper.Map<List<StudentFiles>, List<StudentFileDto>>(entity.Files.ToList());

                var newFiles = entityDto.Files;

                var filesToRemove = currentFiles.Where(cet => !newFiles.Select(f => f.Id).Contains(cet.FileId)).ToList();

                var filesToAdd = newFiles
                    .Where(nt => !currentFiles.Select(uec => uec.FileId).Contains(nt.Id))
                    .Select(sf => new StudentFileDto
                    {
                        FileId = sf.Id,
                        StudentId = entityDto.Id
                    })
                    .ToList();

                if ((await RemoveFilesFromStudent(filesToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentFileDto>> removeResponse))
                {
                    return await ActionResponse<StudentDto>.ReturnError("Neuspješno micanje dokumenata.");
                }

                if ((await AddFilesToStudent(filesToAdd)).IsNotSuccess(out ActionResponse<List<StudentFileDto>> addResponse, out filesToAdd))
                {
                    return await ActionResponse<StudentDto>.ReturnError("Neuspješno dodavanje dokumenata studfentu.");
                }
                return await ActionResponse<StudentDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dokumenti studenta.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentDto>.ReturnError("Greška prilikom izmjene dokumenata studenta.");
            }
        }

        public async Task<ActionResponse<List<StudentFileDto>>> RemoveFilesFromStudent(List<StudentFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<StudentFileDto>>.ReturnSuccess(null, "Datoteke uspješno maknute sa studenta.");
                entities.ForEach(async file =>
                {
                    if ((await RemoveFileFromStudent(file))
                        .IsNotSuccess(out ActionResponse<StudentFileDto> actionResponse))
                    {
                        response = await ActionResponse<List<StudentFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entities);
                return await ActionResponse<List<StudentFileDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<StudentFileDto>> RemoveFileFromStudent(StudentFileDto entity)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentFiles>().Delete(entity.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<StudentFileDto>.ReturnSuccess(null, "Student upsješno izbrisan iz grupe.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entity);
                return await ActionResponse<StudentFileDto>.ReturnError("Greška prilikom micanja studenta iz grupe.");
            }
        }

        public async Task<ActionResponse<List<StudentFileDto>>> AddFilesToStudent(List<StudentFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<StudentFileDto>>.ReturnSuccess(null, "Datoteka uspješno dodani studentu.");
                entities.ForEach(async s =>
                {
                    if ((await AddFileToStudent(s))
                        .IsNotSuccess(out ActionResponse<StudentFileDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entities);
                return await ActionResponse<List<StudentFileDto>>.ReturnError("Greška prilikom dodavanja datoteka studentu.");
            }
        }

        public async Task<ActionResponse<StudentFileDto>> AddFileToStudent(StudentFileDto file)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentFileDto, StudentFiles>(file);
                unitOfWork.GetGenericRepository<StudentFiles>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<StudentFileDto>
                    .ReturnSuccess(mapper.Map<StudentFiles, StudentFileDto>(entityToAdd),
                    "Datoteka uspješno dodana studentu.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, file);
                return await ActionResponse<StudentFileDto>.ReturnError("Greška prilikom dodavanja datoteke studentu.");
            }
        }

        public async Task<ActionResponse<StudentDto>> UpdateEnrollmentDate(StudentDto entityDto)
        {
            try
            {
                if ((await GetById(entityDto.Id)).IsNotSuccess(out ActionResponse<StudentDto> getResponse, out StudentDto student))
                {
                    return getResponse;
                }

                if (string.IsNullOrEmpty(student.EnrolmentDate))
                {
                    student.EnrolmentDate = entityDto.EnrolmentDate.ToString();
                }

                return await Update(student);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentDto>.ReturnError($"Greška prilikom ažuriranja datuma upisa za studenta.");
            }
        }
    }
}
