using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class StudentRegisterService : IStudentRegisterService
    {
        #region Ctors and Members

        private const string registerIncludes = "StudentRegisterEntries";
        private const string entryIncludes = "EducationProgram.EducationGroup,StudentsInGroups.Student,StudentsInGroups.StudentGroup,StudentsInGroups.Student.StudentsInGroups.StudentRegisterEntry";
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public StudentRegisterService(IMapper mapper,
            IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        #region Student Register

        #region Readers

        public async Task<ActionResponse<StudentRegisterDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentRegister>()
                    .FindBy(c => c.Id == id, includeProperties: registerIncludes);

                var entityDto = mapper.Map<StudentRegister, StudentRegisterDto>(entity);

                var fullList = Enumerable.Range((entity.BookNumber * 200) - 199, 200).ToList();
                if (!entity.StudentRegisterEntries.Any())
                {
                    entityDto.FreeStudentRegisterNumbers = fullList;
                }
                else
                {
                    var minStudentNumber = entity.StudentRegisterEntries.Min(r => r.StudentRegisterNumber);
                    var maxStudentNumber = entity.StudentRegisterEntries.Max(r => r.StudentRegisterNumber);
                    List<int> realList = entity.StudentRegisterEntries.Select(r => r.StudentRegisterNumber).ToList();

                    var missingNums = fullList.Except(realList).ToList();
                    entityDto.FreeStudentRegisterNumbers = missingNums;
                }

                return await ActionResponse<StudentRegisterDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom dohvata matične knjige.");
            }
        }

        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<StudentRegister>()
                    .GetAll();
                return await ActionResponse<List<StudentRegisterDto>>
                    .ReturnSuccess(mapper.Map<List<StudentRegister>, List<StudentRegisterDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterDto>>.ReturnError("Greška prilikom dohvata svih matičnih knjiga.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<StudentRegister>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja matičnih knjiga.");
            }
        }

        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAllNotFull()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<StudentRegister>()
                    .GetAll(sr => !sr.Full, includeProperties: registerIncludes)
                    .ToList();

                var entityDtos = mapper.Map<List<StudentRegister>, List<StudentRegisterDto>>(entities);

                entities.ForEach(sr =>
                {
                    var fullList = Enumerable.Range((sr.BookNumber * 200) - 199, 200).ToList();
                    if (!sr.StudentRegisterEntries.Any())
                    {
                        entityDtos.FirstOrDefault(e => e.Id == sr.Id).FreeStudentRegisterNumbers = fullList;
                    }
                    else
                    {
                        var minStudentNumber = sr.StudentRegisterEntries.Min(r => r.StudentRegisterNumber);
                        var maxStudentNumber = sr.StudentRegisterEntries.Max(r => r.StudentRegisterNumber);
                        List<int> realList = sr.StudentRegisterEntries.Select(r => r.StudentRegisterNumber).ToList();

                        var missingNums = fullList.Except(realList).ToList();
                        entityDtos.FirstOrDefault(e => e.Id == sr.Id).FreeStudentRegisterNumbers = missingNums;
                    }
                });

                return await ActionResponse<List<StudentRegisterDto>>.ReturnSuccess(entityDtos);
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterDto>>.ReturnError("Greška prilikom dohvata svih matičnih knjiga.");
            }
        }

        [CacheRefreshSource(typeof(StudentRegisterDto))]
        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAllForCache()
        {
            try
            {

                var entities = unitOfWork.GetGenericRepository<StudentRegister>()
                    .GetAll(includeProperties: registerIncludes);

                return await ActionResponse<List<StudentRegisterDto>>.ReturnSuccess(mapper.Map<List<StudentRegisterDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterDto>>.ReturnError("Greška prilikom dohvata svih matičnih knjiga.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<StudentRegisterDto> studentRegisters = new List<StudentRegisterDto>();
                var cachedResponse = await cacheService.GetFromCache<List<StudentRegisterDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out studentRegisters))
                {
                    studentRegisters = (await GetAll()).GetData();
                }

                var pagedResult = await studentRegisters.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<StudentRegisterDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<StudentRegisterDto>>.ReturnError("Greška prilikom dohvata straničnih podataka matične knjige.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<StudentRegisterDto>> Insert(StudentRegisterDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentRegisterDto, StudentRegister>(entityDto);
                unitOfWork.GetGenericRepository<StudentRegister>().Add(entityToAdd);
                unitOfWork.Save();

                return await ActionResponse<StudentRegisterDto>.ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom upisa matične knjige.");
            }
            finally
            {
                await cacheService.RefreshCache<List<StudentRegisterDto>>();
            }
        }

        public async Task<ActionResponse<StudentRegisterDto>> Update(StudentRegisterDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<StudentRegisterDto, StudentRegister>(entityDto);
                unitOfWork.GetGenericRepository<StudentRegister>().Update(entityToUpdate);
                unitOfWork.Save();

                return await ActionResponse<StudentRegisterDto>.ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom upisa matične knjige.");
            }
            finally
            {
                await cacheService.RefreshCache<List<StudentRegisterDto>>();
            }
        }

        public async Task<ActionResponse<StudentRegisterEntryInsertRequest>> PrepareForInsert(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                if ((await cacheService.GetFromCache<List<StudentRegisterEntryDto>>())
                    .IsNotSuccess(out ActionResponse<List<StudentRegisterEntryDto>> registerEntryResponse, out List<StudentRegisterEntryDto> entries))
                {
                    if ((await GetAllEntries()).IsNotSuccess(out registerEntryResponse, out entries))
                    {
                        return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError("Greška prilikom dohvata postojećih zapisa matičnih knjiga.");
                    }
                }

                var similarEntries = entries.Where(p => p.EducationProgramId == request.EducationProgramId);
                var alreadyInserted = similarEntries.Any(sre => sre.StudentsInGroups.StudentId == request.StudentId);

                if (alreadyInserted)
                {
                    return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnWarning("Ovaj student već je unuesen u matičnu knjigu za izabrani program. Molimo provjerite podatke.");
                }

                if ((await cacheService.GetFromCache<List<StudentRegisterDto>>())
                    .IsNotSuccess(out ActionResponse<List<StudentRegisterDto>> registerResponse, out List<StudentRegisterDto> registers))
                {
                    if ((await GetAll()).IsNotSuccess(out registerResponse, out registers))
                    {
                        return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError("Greška prilikom dohvata postojećih matičnih knjiga.");
                    }
                }

                if (!request.BookNumber.HasValue)
                {
                    if ((await GetAllNotFull()).IsNotSuccess(out registerResponse, out List<StudentRegisterDto> notFullRegisters))
                    {
                        return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError(registerResponse.Message);
                    }

                    if (notFullRegisters.Any())
                    {
                        var selectedBook = registers.OrderByDescending(b => b.BookNumber).FirstOrDefault();
                        request.BookNumber = selectedBook.BookNumber;
                        request.BookId = selectedBook.Id;
                    }
                    else
                    {
                        var minBookNumber = registers.Min(r => r.BookNumber) ?? 0;
                        var maxBookNumber = registers.Max(r => r.BookNumber) ?? 0;
                        List<int> fullList = Enumerable.Range(minBookNumber, maxBookNumber - minBookNumber + 1).ToList();
                        List<int> realList = registers.Select(r => r.BookNumber.Value).ToList();

                        var missingNums = fullList.Except(realList).ToList();
                        var nextAvailableNumber = missingNums.Min() + 1;

                        request.BookNumber = nextAvailableNumber;
                        request.CreateNewBook = true;
                    }
                }
                else
                {
                    var chosenBook = unitOfWork.GetGenericRepository<StudentRegister>()
                        .FindBy(b => b.BookNumber == request.BookNumber);

                    if (chosenBook == null)
                    {
                        request.CreateNewBook = true;
                    }
                    request.BookId = chosenBook?.Id;
                }

                if (!request.StudentRegisterNumber.HasValue)
                {
                    var fullList = Enumerable.Range((request.BookNumber.Value * 200) - 199, 200).ToList();
                    if (request.CreateNewBook)
                    {
                        request.StudentRegisterNumber = fullList.Min();
                    }
                    else
                    {
                        if ((await GetById(request.BookId.Value))
                            .IsNotSuccess(out ActionResponse<StudentRegisterDto> bookResponse, out StudentRegisterDto registerDto))
                        {
                            return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError(bookResponse.Message);
                        }
                        request.StudentRegisterNumber = registerDto.FreeStudentRegisterNumbers.Min();
                    }
                }
                return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnSuccess(request);
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError("Greška prilikom pripreme za unos matične knjige.");
            }
        }

        #endregion Writers

        #endregion Student Register

        #region StudentRegisterEntryd

        #region Readers

        public async Task<ActionResponse<StudentRegisterEntryDto>> GetEntryById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .FindBy(c => c.Id == id, includeProperties: entryIncludes);
                return await ActionResponse<StudentRegisterEntryDto>
                    .ReturnSuccess(mapper.Map<StudentRegisterEntry, StudentRegisterEntryDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom dohvata zapisa matične knjige.");
            }
        }

        public async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntries()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .GetAll(includeProperties: entryIncludes);
                return await ActionResponse<List<StudentRegisterEntryDto>>
                    .ReturnSuccess(mapper.Map<List<StudentRegisterEntry>, List<StudentRegisterEntryDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata svih zapisa matičnih knjiga.");
            }
        }

        [CacheRefreshSource(typeof(StudentRegisterEntryDto))]
        public async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntriesForCache()
        {
            try
            {
                if ((await GetAllEntries())
                    .IsNotSuccess(out ActionResponse<List<StudentRegisterEntryDto>> response, out List<StudentRegisterEntryDto> entityDtos))
                {
                    return response;
                }

                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnSuccess(entityDtos);
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata svih zapisa matičnih knjiga.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<StudentRegisterEntryDto> entityDtos = new List<StudentRegisterEntryDto>();
                var cachedResponse = await cacheService.GetFromCache<List<StudentRegisterEntryDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out entityDtos))
                {
                    entityDtos = (await GetAllEntries()).GetData();
                }

                var pagedResult = await entityDtos.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<StudentRegisterEntryDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka zapisa matične knjige.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesByBookIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<StudentRegisterEntryDto> entityDtos = new List<StudentRegisterEntryDto>();
                var cachedResponse = await cacheService.GetFromCache<List<StudentRegisterEntryDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out entityDtos))
                {
                    entityDtos = (await GetAllEntries()).GetData();
                }

                var pagedResult = await entityDtos
                    .Where(e => e.StudentRegisterId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<StudentRegisterEntryDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka zapisa matične knjige.");
            }
        }

        private async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetEntriesForStudentAndProgram(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var insertedStudents = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .GetAll(sre => sre.EducationProgramId == request.EducationProgramId.Value
                        && sre.StudentsInGroups.StudentId == request.StudentId.Value
                        && sre.Status == DatabaseEntityStatusEnum.Active)
                        .ToList();
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnSuccess(mapper.Map<List<StudentRegisterEntryDto>>(insertedStudents));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata zapisa koji sadrže traženi program i studenta.");
            }
        }

        private async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetEntriesForStudentNumberAndBookNumber(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var insertedStudents = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .GetAll(sre => sre.StudentRegisterNumber == request.StudentRegisterNumber
                        && sre.StudentRegister.BookNumber == request.BookNumber
                        && sre.Status == DatabaseEntityStatusEnum.Active)
                        .ToList();
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnSuccess(mapper.Map<List<StudentRegisterEntryDto>>(insertedStudents));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata zapisa koji sadrže traženi program i studenta.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<StudentRegisterEntryDto>> InsertEntry(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if ((await GetEntriesForStudentAndProgram(request))
                        .IsNotSuccess(out ActionResponse<List<StudentRegisterEntryDto>> checkResponse, out List<StudentRegisterEntryDto> alreadyExisting))
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(checkResponse.Message);
                    }

                    if (alreadyExisting.Any())
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Nemoguće unjeti novi zapis za kombinaciju studenta i edukacijskog programa, takav zapis već postoji.");
                    }

                    if ((await GetEntriesForStudentNumberAndBookNumber(request))
                        .IsNotSuccess(out checkResponse, out alreadyExisting))
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(checkResponse.Message);
                    }

                    if (alreadyExisting.Any())
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Nemoguće unjeti novi zapis jer zapis sa istim brojem studenta već postoji unutar izabrane knjige.");
                    }

                    if ((await PrepareForInsert(request))
                        .IsNotSuccess(out ActionResponse<StudentRegisterEntryInsertRequest> prepareResponse, out request))
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(prepareResponse.Message);
                    }

                    if (request.CreateNewBook)
                    {
                        var entityDto = new StudentRegisterDto
                        {
                            BookNumber = request.BookNumber
                        };

                        if ((await Insert(entityDto)).IsNotSuccess(out ActionResponse<StudentRegisterDto> bookInsertResponse, out entityDto))
                        {
                            return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookInsertResponse.Message);
                        }
                        request.BookId = entityDto.Id;
                    }

                    var studentInGroupId = unitOfWork.GetGenericRepository<StudentsInGroups>()
                        .FindBy(sig => sig.StudentId == request.StudentId.Value
                        && sig.StudentGroupId == request.StudentGroupId).Id;

                    var entityToAdd = new StudentRegisterEntry
                    {
                        StudentRegisterId = request.BookId.Value,
                        EducationProgramId = request.EducationProgramId.Value,
                        StudentsInGroupsId = studentInGroupId,
                        StudentRegisterNumber = request.StudentRegisterNumber.Value,
                        Notes = request.Notes,
                        EntryDate = request.EntryDate,
                        ExamDate = request.ExamDate,
                        ExamDateNumber = request.ExamDateNumber
                    };

                    unitOfWork.GetGenericRepository<StudentRegisterEntry>().Add(entityToAdd);
                    unitOfWork.Save();

                    if ((await GetById(request.BookId.Value))
                        .IsNotSuccess(out ActionResponse<StudentRegisterDto> bookResponse, out StudentRegisterDto registerDto))
                    {
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookResponse.Message);
                    }

                    if (registerDto.NumberOfEntries >= 200)
                    {
                        registerDto.Full = true;
                        if ((await Update(registerDto)).IsNotSuccess(out bookResponse, out registerDto))
                        {
                            return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookResponse.Message);
                        }
                    }

                    scope.Complete();
                    return await ActionResponse<StudentRegisterEntryDto>.ReturnSuccess(mapper.Map<StudentRegisterEntry, StudentRegisterEntryDto>(entityToAdd));
                }
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom upisa u matičnu knjigu.");
            }
            finally
            {
                var registerTask = cacheService.RefreshCache<List<StudentRegisterDto>>();
                var entryTask = cacheService.RefreshCache<List<StudentRegisterEntryDto>>();

                await Task.WhenAll(registerTask, entryTask);
            }
        }

        public async Task<ActionResponse<StudentRegisterEntryDto>> UpdateEntry(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<StudentRegisterEntry>().FindSingle(request.EntryId.Value);
                entityToUpdate.Notes = request.Notes;
                unitOfWork.GetGenericRepository<StudentRegisterEntry>().Update(entityToUpdate);
                unitOfWork.Save();

                return await GetEntryById(request.EntryId.Value);
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom ažuriranja zapisa matične knjige.");
            }
            finally
            {
                await cacheService.RefreshCache<List<StudentRegisterEntryDto>>();
            }
        }

        #endregion Writers

        #endregion StudentRegisterEntry
    }
}
