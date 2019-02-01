using AutoMapper;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class InstitutionService : IInstitutionService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public InstitutionService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<InstitutionDto>> GetInstitution()
        {
            try
            {
                var institution = unitOfWork.GetGenericRepository<Institution>()
                    .GetAll(includeProperties: "Principal,Country,Region").FirstOrDefault();
                return await ActionResponse<InstitutionDto>
                    .ReturnSuccess(mapper.Map<Institution, InstitutionDto>(institution));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<InstitutionDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Insert(InstitutionDto institution)
        {
            try
            {
                var institutionToAdd = mapper.Map<InstitutionDto, Institution>(institution);
                unitOfWork.GetGenericRepository<Institution>().Add(institutionToAdd);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(institutionToAdd).Reference(p => p.Principal).Load();
                unitOfWork.GetContext().Entry(institutionToAdd).Reference(p => p.Logo).Load();
                unitOfWork.GetContext().Entry(institutionToAdd).Reference(p => p.Country).Load();
                unitOfWork.GetContext().Entry(institutionToAdd).Reference(p => p.Region).Load();
                unitOfWork.GetContext().Entry(institutionToAdd).Reference(p => p.City).Load();
                return await ActionResponse<InstitutionDto>
                    .ReturnSuccess(mapper.Map<Institution, InstitutionDto>(institutionToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom dohvata podataka za instituciju.");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Update(InstitutionDto institution)
        {
            try
            {
                var institutionToUpdate = mapper.Map<InstitutionDto, Institution>(institution);
                unitOfWork.GetGenericRepository<Institution>().Update(institutionToUpdate);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(institutionToUpdate).Reference(p => p.Principal).Load();
                unitOfWork.GetContext().Entry(institutionToUpdate).Reference(p => p.Logo).Load();
                unitOfWork.GetContext().Entry(institutionToUpdate).Reference(p => p.Country).Load();
                unitOfWork.GetContext().Entry(institutionToUpdate).Reference(p => p.Region).Load();
                unitOfWork.GetContext().Entry(institutionToUpdate).Reference(p => p.City).Load();
                return await ActionResponse<InstitutionDto>
                    .ReturnSuccess(mapper.Map<Institution, InstitutionDto>(institutionToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<InstitutionDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Institution>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<InstitutionDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<InstitutionDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
