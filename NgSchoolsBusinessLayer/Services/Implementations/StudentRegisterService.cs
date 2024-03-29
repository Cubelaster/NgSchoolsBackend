﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Core.Enums.Common;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using Core.Utilities.Attributes;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class StudentRegisterService : IStudentRegisterService
    {
        #region Ctors and Members

        private const string registerIncludes = "StudentRegisterEntries,StudentRegisterEntries.EducationProgram,StudentRegisterEntries.StudentsInGroups";
        private const string entryIncludes = "StudentRegister,EducationProgram,EducationProgram.EducationGroup,EducationProgram.Subjects" +
            ",StudentsInGroups.Student,StudentsInGroups.StudentGroup" +
            ",StudentsInGroups.Student.AddressCity,StudentsInGroups.Student.AddressCountry,StudentsInGroups.Student.AddressRegion" +
            ",StudentsInGroups.Student.CountryOfBirth,StudentsInGroups.Student.RegionOfBirth,StudentsInGroups.Student.CityOfBirth" +
            ",StudentsInGroups.StudentGroup.Director.UserDetails,StudentsInGroups.StudentGroup.Director.UserDetails.Signature" +
            ",StudentsInGroups.StudentGroup.EducationLeader.UserDetails,StudentsInGroups.StudentGroup.EducationLeader.UserDetails.Signature" +
            ",StudentsInGroups.Student.StudentsInGroups.StudentRegisterEntry";

        private readonly IStudentService studentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public StudentRegisterService(IStudentService studentService, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this.studentService = studentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        #region Student Register

        #region Readers

        public async Task<ActionResponse<StudentRegisterDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentRegister>()
                    .ReadAllActiveAsQueryable()
                    .Where(c => c.Id == id)
                    .Include(e => e.StudentRegisterEntries)
                        .ThenInclude(e => e.EducationProgram)
                    .Include(e => e.StudentRegisterEntries)
                        .ThenInclude(e => e.StudentsInGroups)
                    .FirstOrDefault();

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
                var query = unitOfWork.GetGenericRepository<StudentRegister>()
                    .ReadAllActiveAsQueryable();

                return await ActionResponse<List<StudentRegisterDto>>
                    .ReturnSuccess(mapper.ProjectTo<StudentRegisterDto>(query).ToList());
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
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<StudentRegister>().ReadAllActiveAsQueryable().Count());
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
                    .ReadAllActiveAsQueryable()
                    .Where(sr => !sr.Full)
                    .Include(e => e.StudentRegisterEntries)
                        .ThenInclude(e => e.EducationProgram)
                    .Include(e => e.StudentRegisterEntries)
                        .ThenInclude(e => e.StudentsInGroups)
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

        //[CacheRefreshSource(typeof(StudentRegisterDto))]
        //public async Task<ActionResponse<List<StudentRegisterDto>>> GetAllForCache()
        //{
        //    try
        //    {

        //        var entities = unitOfWork.GetGenericRepository<StudentRegister>()
        //            .ReadAllActiveAsQueryable()
        //            .Include(e => e.StudentRegisterEntries)
        //                .ThenInclude(e => e.EducationProgram)
        //            .Include(e => e.StudentRegisterEntries)
        //                .ThenInclude(e => e.StudentsInGroups)
        //            .ToList();

        //        return await ActionResponse<List<StudentRegisterDto>>.ReturnSuccess(mapper.Map<List<StudentRegisterDto>>(entities));
        //    }
        //    catch (Exception)
        //    {
        //        return await ActionResponse<List<StudentRegisterDto>>.ReturnError("Greška prilikom dohvata svih matičnih knjiga.");
        //    }
        //}

        public async Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<StudentRegister>()
                    .ReadAllActiveAsQueryable();

                var pagedResult = await query.GetPaged(pagedRequest, true);

                var realResult = new PagedResult<StudentRegisterDto>
                {
                    CurrentPage = pagedResult.CurrentPage,
                    PageCount = pagedResult.PageCount,
                    PageSize = pagedResult.PageSize,
                    RowCount = pagedResult.RowCount,
                    Results = mapper.ProjectTo<StudentRegisterDto>(pagedResult.ResultQuery).ToList()
                };

                return await ActionResponse<PagedResult<StudentRegisterDto>>.ReturnSuccess(realResult);
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entityToAdd = mapper.Map<StudentRegisterDto, StudentRegister>(entityDto);
                    unitOfWork.GetGenericRepository<StudentRegister>().Add(entityToAdd);
                    unitOfWork.Save();

                    scope.Complete();

                    return await ActionResponse<StudentRegisterDto>.ReturnSuccess(mapper.Map(entityToAdd, entityDto));
                }
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom upisa matične knjige.");
            }
        }

        public async Task<ActionResponse<StudentRegisterDto>> Update(StudentRegisterDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entityToUpdate = mapper.Map<StudentRegisterDto, StudentRegister>(entityDto);
                    unitOfWork.GetGenericRepository<StudentRegister>().Update(entityToUpdate);
                    unitOfWork.Save();

                    scope.Complete();

                    return await ActionResponse<StudentRegisterDto>.ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
                }
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom upisa matične knjige.");
            }
        }

        public async Task<ActionResponse<StudentRegisterDto>> Delete(int id)
        {
            try
            {
                var response = await ActionResponse<StudentRegisterDto>.ReturnSuccess(null, "Matična knjiga uspješno obrisana");

                unitOfWork.GetGenericRepository<StudentRegister>().Delete(id);

                unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .GetAll(sre => sre.StudentRegisterId == id)
                    .ForEach(async sre =>
                    {
                        if ((await DeleteEntry(sre.Id)).IsNotSuccess(out ActionResponse<StudentRegisterEntryDto> deleteResponse))
                        {
                            response = await ActionResponse<StudentRegisterDto>.ReturnError(deleteResponse.Message);
                        }
                    });

                if (response.IsNotSuccess())
                {
                    return response;
                }

                unitOfWork.Save();

                return response;

            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterDto>.ReturnError("Greška prilikom brisanja matične knjige.");
            }
        }

        public async Task<ActionResponse<StudentRegisterEntryInsertRequest>> PrepareForInsert(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var alreadyInserted = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.EducationProgramId == request.EducationProgramId)
                    .Any(e => e.StudentsInGroups.StudentId == request.StudentId);

                if (alreadyInserted)
                {
                    return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnWarning("Ovaj student već je unuesen u matičnu knjigu za izabrani program. Molimo provjerite podatke.");
                }


                ActionResponse<List<StudentRegisterDto>> registerResponse;

                if (!request.BookNumber.HasValue)
                {
                    if ((await GetAllNotFull()).IsNotSuccess(out registerResponse, out List<StudentRegisterDto> notFullRegisters))
                    {
                        return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError(registerResponse.Message);
                    }

                    if (notFullRegisters.Any(sr => sr.BookYear == request.BookYear))
                    {
                        var selectedBook = notFullRegisters.OrderByDescending(b => b.BookNumber).FirstOrDefault();
                        request.BookNumber = selectedBook.BookNumber;
                        request.BookYear = selectedBook.BookYear;
                        request.BookId = selectedBook.Id;
                    }
                    else
                    {
                        if ((await GetAll()).IsNotSuccess(out registerResponse, out List<StudentRegisterDto> registers))
                        {
                            return await ActionResponse<StudentRegisterEntryInsertRequest>.ReturnError("Greška prilikom dohvata postojećih matičnih knjiga.");
                        }

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
                        .ReadAllActiveAsQueryable()
                        .Where(b => b.BookNumber == request.BookNumber && b.BookYear == request.BookYear)
                        .FirstOrDefault();

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

        #region StudentRegisterEntry

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
                    .ReadAll(includeProperties: entryIncludes)
                    .ToList();

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
                var entityDtos = (await GetAllEntries()).GetData();

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
                var entityDtos = (await GetAllEntries()).GetData();

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

        public async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntriesByBookId(int id)
        {
            try
            {
                var entityDtos = (await GetAllEntries()).GetData();

                entityDtos = entityDtos.Where(sre => sre.StudentRegisterId == id).ToList();
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnSuccess(entityDtos);
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterEntryDto>>.ReturnError("Greška prilikom dohvata svih zapisa matičnih knjiga za odabranu knjigu.");
            }
        }

        private async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetEntriesForStudentAndProgram(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var insertedStudents = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .ReadAllActiveAsQueryable()
                    .Where(sre => sre.EducationProgramId == request.EducationProgramId.Value
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

        private async Task<ActionResponse<StudentRegisterEntryDto>> GetEntryForStudentNumberAndBookNumberAndBookYear(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var insertedStudent = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .ReadAllActiveAsQueryable()
                    .Where(sre => sre.StudentRegisterNumber == request.StudentRegisterNumber
                        && sre.StudentRegister.BookNumber == request.BookNumber
                        && sre.StudentRegister.BookYear == request.BookYear
                        && sre.Status == DatabaseEntityStatusEnum.Active)
                    .FirstOrDefault();

                return await ActionResponse<StudentRegisterEntryDto>.ReturnSuccess(mapper.Map<StudentRegisterEntryDto>(insertedStudent));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom dohvata zapisa po knjizi i broju studenta.");
            }
        }

        private async Task<ActionResponse<StudentRegisterEntryDto>> GetEntryForStudentNumberAndBookNumberAndBookYearDetailed(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var insertedStudent = unitOfWork.GetGenericRepository<StudentRegisterEntry>()
                    .GetAll(sre => sre.StudentRegisterNumber == request.StudentRegisterNumber
                        && sre.StudentRegister.BookNumber == request.BookNumber
                        && sre.StudentRegister.BookYear == request.BookYear
                        && sre.Status == DatabaseEntityStatusEnum.Active, includeProperties: "StudentsInGroups.Student," +
                        "StudentsInGroups.Student.CityOfBirth,StudentsInGroups.Student.CountryOfBirth,StudentsInGroups.Student.RegionOfBirth," +
                        "StudentsInGroups.Student.MunicipalityOfBirth,StudentsInGroups.Student.AddressCountry," +
                        "StudentsInGroups.Student.AddressCity,StudentsInGroups.Student.AddressRegion,StudentsInGroups.Student.AddressMunicipality," +
                        "StudentsInGroups.StudentGroup," +
                        "StudentsInGroups.StudentGroup.ClassLocation.Country, StudentsInGroups.StudentGroup.ClassLocation.Region," +
                        "StudentsInGroups.StudentGroup.ClassLocation.Municipality, StudentsInGroups.StudentGroup.ClassLocation.City," +
                        "EducationProgram.EducationGroup, EducationProgram.Subjects," +
                        "StudentsInGroups.StudentGroup.Director.UserDetails, StudentsInGroups.StudentGroup.Director.UserDetails.Signature," +
                        "StudentsInGroups.StudentGroup.EducationLeader.UserDetails, StudentsInGroups.StudentGroup.EducationLeader.UserDetails.Signature")
                        .FirstOrDefault();
                return await ActionResponse<StudentRegisterEntryDto>.ReturnSuccess(mapper.Map<StudentRegisterEntryDto>(insertedStudent));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom dohvata zapisa po knjizi i broju studenta.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<StudentRegisterEntryDto>> InsertEntry(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.MaximumTimeout
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    if ((await GetEntriesForStudentAndProgram(request))
                        .IsNotSuccess(out ActionResponse<List<StudentRegisterEntryDto>> checkResponse, out List<StudentRegisterEntryDto> alreadyExisting))
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(checkResponse.Message);
                    }

                    if (alreadyExisting.Any())
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Nemoguće unjeti novi zapis za kombinaciju studenta i edukacijskog programa, takav zapis već postoji.");
                    }

                    if ((await GetEntryForStudentNumberAndBookNumberAndBookYear(request))
                        .IsNotSuccess(out ActionResponse<StudentRegisterEntryDto> registerEntryResponse, out StudentRegisterEntryDto existingEntry))
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(registerEntryResponse.Message);
                    }

                    if (alreadyExisting.Any())
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Nemoguće unjeti novi zapis jer zapis sa istim brojem studenta već postoji unutar izabrane knjige.");
                    }

                    if ((await PrepareForInsert(request))
                        .IsNotSuccess(out ActionResponse<StudentRegisterEntryInsertRequest> prepareResponse, out request))
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(prepareResponse.Message);
                    }

                    if (request.CreateNewBook)
                    {
                        var entityDto = new StudentRegisterDto
                        {
                            BookNumber = request.BookNumber,
                            BookYear = request.BookYear,
                        };

                        if ((await Insert(entityDto)).IsNotSuccess(out ActionResponse<StudentRegisterDto> bookInsertResponse, out entityDto))
                        {
                            scope.Dispose();
                            return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookInsertResponse.Message);
                        }
                        request.BookId = entityDto.Id;
                    }

                    var studentInGroup = unitOfWork.GetGenericRepository<StudentsInGroups>()
                        .ReadAllActiveAsQueryable()
                        .Where(sig => sig.StudentId == request.StudentId.Value
                            && sig.StudentGroupId == request.StudentGroupId)
                        .FirstOrDefault();

                    if (studentInGroup == null)
                    {
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Specificirani student još ne postoji u grupi. Molimo prvo spremite studenta u grupu.");
                    }

                    var entityToAdd = new StudentRegisterEntry
                    {
                        StudentRegisterId = request.BookId.Value,
                        EducationProgramId = request.EducationProgramId.Value,
                        StudentsInGroupsId = studentInGroup.Id,
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
                        scope.Dispose();
                        return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookResponse.Message);
                    }

                    if (registerDto.NumberOfEntries >= 200)
                    {
                        if (request.CreateNewBook)
                        {
                            entityToAdd.StudentRegister.Full = true;
                            unitOfWork.Save();
                        }
                        else
                        {
                            registerDto.Full = true;
                            if ((await Update(registerDto)).IsNotSuccess(out bookResponse, out registerDto))
                            {
                                scope.Dispose();
                                return await ActionResponse<StudentRegisterEntryDto>.ReturnError(bookResponse.Message);
                            }
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
            //finally
            //{
            //var registerTask = cacheService.RefreshCache<List<StudentRegisterDto>>();
            //var entryTask = cacheService.RefreshCache<List<StudentRegisterEntryDto>>();

            //await Task.WhenAll(registerTask, entryTask);
            //}
        }

        public async Task<ActionResponse<StudentRegisterEntryDto>> UpdateEntry(StudentRegisterEntryInsertRequest request)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<StudentRegisterEntry>().FindSingle(request.EntryId.Value);

                entityToUpdate.Notes = request.Notes;
                entityToUpdate.EducationProgramId = request.EducationProgramId.Value;
                entityToUpdate.StudentRegisterId = request.BookId.Value;
                entityToUpdate.StudentRegisterNumber = request.StudentRegisterNumber.Value;
                entityToUpdate.EntryDate = request.EntryDate;
                entityToUpdate.ExamDate = request.ExamDate;
                entityToUpdate.ExamDateNumber = request.ExamDateNumber;

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
                //await cacheService.RefreshCache<List<StudentRegisterEntryDto>>();
            }
        }

        public async Task<ActionResponse<StudentRegisterEntryDto>> DeleteEntry(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentRegisterEntry>().Delete(id);
                unitOfWork.Save();

                //await cacheService.RefreshCache<List<StudentRegisterEntryDto>>();

                return await ActionResponse<StudentRegisterEntryDto>.ReturnSuccess(null, "Brisanje zapisa iz matične knjige uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentRegisterEntryDto>.ReturnError("Greška prilikom brisanja zapisa u matičnoj knjizi.");
            }
            //finally
            //{
            //    await cacheService.RefreshCache<List<StudentRegisterEntryDto>>();
            //}
        }

        #endregion Writers

        #endregion StudentRegisterEntry

        #region Print

        public async Task<ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>> GetPrintDataForBookAndEntriesRange(StudentRegisterPrintForRangeRequest request)
        {
            try
            {
                var studentRegisterEntryFailResponses = new List<ActionResponse<StudentRegisterEntryDto>>();
                var educationProgramsFailResponses = new List<ActionResponse<StudentEducationProgramsPrintModel>>();
                var printData = new List<StudentRegisterPrintDataAggregatedDto>();

                var range = Enumerable.Range(request.StudentRegisterNumberRangeFrom, request.StudentRegisterNumberRangeTo - request.StudentRegisterNumberRangeFrom);

                await Task.WhenAll(range.Select(async number =>
                 {
                     if ((await GetEntryForStudentNumberAndBookNumberAndBookYearDetailed(new StudentRegisterEntryInsertRequest
                     {
                         BookNumber = request.BookNumber,
                         BookYear = request.BookYear,
                         StudentRegisterNumber = number
                     })).IsNotSuccess(out ActionResponse<StudentRegisterEntryDto> registerEntryResponse, out StudentRegisterEntryDto studentRegisterEntry))
                     {
                         studentRegisterEntryFailResponses.Add(registerEntryResponse);
                     }
                     else if (studentRegisterEntry != null)
                     {
                         if ((await studentService.GetStudentsEducationPrograms(studentRegisterEntry.Student.Id))
                         .IsNotSuccess(out ActionResponse<StudentEducationProgramsPrintModel> studentEducationProgramsResponse, out StudentEducationProgramsPrintModel studentEducationPrograms))
                         {
                             educationProgramsFailResponses.Add(studentEducationProgramsResponse);
                         }
                         else
                         {
                             printData.Add(new StudentRegisterPrintDataAggregatedDto
                             {
                                 StudentRegisterEntry = studentRegisterEntry,
                                 StudentEducationPrograms = studentEducationPrograms
                             });
                         }
                     }
                 }));

                var anyRegisterFails = studentRegisterEntryFailResponses.Any();
                var anyEduProgramsFails = educationProgramsFailResponses.Any();

                if (!anyRegisterFails && !anyEduProgramsFails)
                {
                    return await ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>.ReturnSuccess(printData);
                }
                else
                {
                    if ((anyRegisterFails && studentRegisterEntryFailResponses.All(r => r.ActionResponseType != ActionResponseTypeEnum.Success))
                    || (anyEduProgramsFails && educationProgramsFailResponses.All(r => r.ActionResponseType != ActionResponseTypeEnum.Success)))
                    {
                        return await ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>.ReturnError("Greška prilikom dohvata podataka.", printData);
                    }

                    return await ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>.ReturnWarning("Postoje greške prilikom dohvata zapisa matične knjige.", printData);
                }
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>.ReturnError("Greška prilikom dohvata zapisa.");
            }
        }

        #endregion Print
    }
}
