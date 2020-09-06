using AutoMapper;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Models.Common;
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
    public class IssuedPrintService : IIssuedPrintService
    {
        #region Ctors and Members

        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public IssuedPrintService(IConfiguration configuration,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        #region Readers

        public async Task<ActionResponse<IssuedPrintDto>> GetById(int id)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<IssuedPrint>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.Id == id);

                var entity = mapper.ProjectTo<IssuedPrintDto>(query).FirstOrDefault();

                return await ActionResponse<IssuedPrintDto>.ReturnSuccess(entity);
            }
            catch (Exception)
            {
                return await ActionResponse<IssuedPrintDto>.ReturnError("Greška prilikom dohvata izdanih printeva.");
            }
        }

        public async Task<ActionResponse<List<IssuedPrintDto>>> GetAll()
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<IssuedPrint>()
                        .ReadAllActiveAsQueryable();

                var entities = mapper.ProjectTo<IssuedPrintDto>(query).ToList();

                return await ActionResponse<List<IssuedPrintDto>>.ReturnSuccess(entities);
            }
            catch (Exception)
            {
                return await ActionResponse<List<IssuedPrintDto>>.ReturnError("Greška prilikom dohvata svih izdanih printeva.");
            }
        }

        public async Task<ActionResponse<List<IssuedPrintDto>>> GetForStudentAndProgram(IssuedPrintDto entityDto)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<IssuedPrint>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.StudentId == entityDto.StudentId && e.EducationProgramId == entityDto.EducationProgramId);

                var entity = mapper.ProjectTo<IssuedPrintDto>(query).ToList();

                return await ActionResponse<List<IssuedPrintDto>>.ReturnSuccess(entity);
            }
            catch (Exception)
            {
                return await ActionResponse<List<IssuedPrintDto>>.ReturnError("Greška prilikom dohvata izdanih duplikata za polaznika.");
            }
        }

        public async Task<ActionResponse<int>> GetForStudentAndProgramTotalDuplicates(IssuedPrintDto entityDto)
        {
            try
            {
                var result = unitOfWork.GetGenericRepository<IssuedPrint>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.StudentId == entityDto.StudentId && e.EducationProgramId == entityDto.EducationProgramId)
                    .Sum(e => e.PrintNumber);

                return await ActionResponse<int>.ReturnSuccess(result);
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja duplikata za polaznika.");
            }
        }

        public async Task<ActionResponse<Dictionary<DateTime, int>>> GetForCurrentYear(SimpleRequestBase request)
        {
            try
            {
                var targetYear = new DateTime((request.DateParam ?? DateTime.Now).Year, 1, 1);

                var query = unitOfWork.GetGenericRepository<IssuedPrint>()
                    .ReadAllActiveAsQueryable()
                    .Where(e => e.DateCreated >= targetYear && e.DateCreated <= targetYear.AddYears(1).AddTicks(-1));

                var data = query
                    .GroupBy(q => new DateTime(q.PrintDate.Year, 1, 1))
                    .ToDictionary(g => g.Key, g => g.Sum(element => element.PrintNumber));

                var initialValues = configuration
                    .GetSection("IssuedPrintsInitialNumber")
                    .Get<Dictionary<string, int>>();

                foreach (var entry in initialValues)
                {
                    var dateKey = DateTime.Parse(entry.Key);
                    if (data.ContainsKey(dateKey))
                    {
                        data[dateKey] = data[dateKey] + entry.Value;
                    }
                    else if(targetYear == dateKey)
                    {
                        data.Add(dateKey, entry.Value);
                    }
                }

                return await ActionResponse<Dictionary<DateTime, int>>.ReturnSuccess(data);
            }
            catch (Exception)
            {
                return await ActionResponse<Dictionary<DateTime, int>>.ReturnError("Greška prilikom dohvata izdanih duplikata za godinu.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<IssuedPrintDto>> Insert(IssuedPrintDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<IssuedPrintDto, IssuedPrint>(entityDto);
                unitOfWork.GetGenericRepository<IssuedPrint>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<IssuedPrintDto>
                    .ReturnSuccess(mapper.Map<IssuedPrint, IssuedPrintDto>(entityToAdd));
            }
            catch (Exception)
            {
                return await ActionResponse<IssuedPrintDto>.ReturnError("Greška prilikom kreiranja izdanog printa.");
            }
        }

        public async Task<ActionResponse<IssuedPrintDto>> Update(IssuedPrintDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<IssuedPrintDto, IssuedPrint>(entityDto);
                unitOfWork.GetGenericRepository<IssuedPrint>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<IssuedPrintDto>
                    .ReturnSuccess(mapper.Map<IssuedPrint, IssuedPrintDto>(entityToUpdate));
            }
            catch (Exception)
            {
                return await ActionResponse<IssuedPrintDto>.ReturnError("Greška prilikom ažuriranja izdanog printa.");
            }
        }

        public async Task<ActionResponse<IssuedPrintDto>> Increment(IssuedPrintDto entityDto)
        {
            try
            {
                var targetYear = new DateTime(entityDto.PrintDate.Year, 1, 1);

                var entityToUpdate = unitOfWork.GetGenericRepository<IssuedPrint>()
                    .GetAllAsQueryable()
                    .Where(e => e.StudentId == entityDto.StudentId
                        && e.EducationProgramId == entityDto.EducationProgramId
                        && e.DateCreated >= targetYear && e.DateCreated <= targetYear.AddYears(1).AddTicks(-1))
                    .FirstOrDefault();

                entityToUpdate.PrintNumber += 1;

                unitOfWork.Save();

                return await ActionResponse<IssuedPrintDto>
                    .ReturnSuccess(mapper.Map<IssuedPrint, IssuedPrintDto>(entityToUpdate));
            }
            catch (Exception)
            {
                return await ActionResponse<IssuedPrintDto>.ReturnError("Greška prilikom inkrementa izdanog printa.");
            }
        }

        public async Task<ActionResponse<IssuedPrintDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<IssuedPrint>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<IssuedPrintDto>.ReturnSuccess(null, "Brisanje izdanog printa uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<IssuedPrintDto>.ReturnError("Greška prilikom brisanja izdanog printa.");
            }
        }

        #endregion Writers
    }
}
