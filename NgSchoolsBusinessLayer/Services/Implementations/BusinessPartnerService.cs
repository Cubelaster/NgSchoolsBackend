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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public BusinessPartnerService(IMapper mapper, ILoggerService loggerService, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<BusinessPartnerDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<BusinessPartner>()
                    .FindBy(c => c.Id == id, includeProperties: "BusinessPartnerContacts,City,Region,Country");
                return await ActionResponse<BusinessPartnerDto>
                    .ReturnSuccess(mapper.Map<BusinessPartner, BusinessPartnerDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<BusinessPartnerDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<BusinessPartner>()
                    .GetAll(bp => bp.IsBusinessPartner,
                    includeProperties: "BusinessPartnerContacts,City,Region,Country");
                return await ActionResponse<List<BusinessPartnerDto>>
                    .ReturnSuccess(mapper.Map<List<BusinessPartner>, List<BusinessPartnerDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<BusinessPartnerDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<PagedResult<BusinessPartnerDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<BusinessPartner>()
                    .GetAllAsQueryable(bp => bp.IsBusinessPartner,
                    includeProperties: "BusinessPartnerContacts,City,Region,Country").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<BusinessPartnerDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<BusinessPartner>, List<BusinessPartnerDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<BusinessPartnerDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<BusinessPartnerDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Insert(BusinessPartnerDto entityDto)
        {
            try
            {
                List<ContactPersonDto> contacts = entityDto.BusinessPartnerContacts != null ?
                    new List<ContactPersonDto>(entityDto.BusinessPartnerContacts) : new List<ContactPersonDto>();

                var entityToAdd = mapper.Map<BusinessPartnerDto, BusinessPartner>(entityDto);
                unitOfWork.GetGenericRepository<BusinessPartner>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                entityDto.BusinessPartnerContacts = contacts;

                if ((await ModifyBusinessPartnerContacts(entityDto))
                    .IsNotSuccess(out ActionResponse<BusinessPartnerDto> contactResponse, out entityDto))
                {
                    return contactResponse;
                }

                if ((await GetById(entityToAdd.Id)).IsNotSuccess(out ActionResponse<BusinessPartnerDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<BusinessPartnerDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Update(BusinessPartnerDto entityDto)
        {
            try
            {
                List<ContactPersonDto> contacts = entityDto.BusinessPartnerContacts != null ?
                    new List<ContactPersonDto>(entityDto.BusinessPartnerContacts) : new List<ContactPersonDto>();

                var entityToUpdate = mapper.Map<BusinessPartnerDto, BusinessPartner>(entityDto);
                unitOfWork.GetGenericRepository<BusinessPartner>().Update(entityToUpdate);
                unitOfWork.Save();

                mapper.Map(entityToUpdate, entityDto);
                entityDto.BusinessPartnerContacts = contacts;

                if ((await ModifyBusinessPartnerContacts(entityDto))
                    .IsNotSuccess(out ActionResponse<BusinessPartnerDto> contactResponse, out entityDto))
                {
                    return contactResponse;
                }

                if ((await GetById(entityToUpdate.Id)).IsNotSuccess(out ActionResponse<BusinessPartnerDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<BusinessPartnerDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<BusinessPartner>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<BusinessPartnerDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<BusinessPartnerDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<BusinessPartnerDto>> ModifyBusinessPartnerContacts(BusinessPartnerDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<BusinessPartner>()
                    .FindBy(e => e.Id == entityDto.Id, includeProperties: "BusinessPartnerContacts");

                var currentContacts = mapper.Map<List<ContactPerson>, List<ContactPersonDto>>(entity.BusinessPartnerContacts.ToList());

                var newContacts = entityDto.BusinessPartnerContacts
                    .Select(c =>
                    {
                        c.BusinessPartnerId = entityDto.Id;
                        return c;
                    }).ToList();

                var contactsToRemove = currentContacts.Where(cet => !newContacts
                    .Select(f => f.Id).Contains(cet.Id)).ToList();

                var contactsToAdd = newContacts
                    .Where(nt => !currentContacts.Select(cd => cd.Id).Contains(nt.Id))
                    .Select(c =>
                    {
                        c.BusinessPartnerId = entityDto.Id;
                        return c;
                    }).ToList();

                var contactsToModify = newContacts.Where(cd => currentContacts
                    .Select(nd => nd.Id).Contains(cd.Id)).ToList();

                if ((await RemoveContacts(contactsToRemove))
                    .IsNotSuccess(out ActionResponse<List<ContactPersonDto>> removeResponse))
                {
                    return await ActionResponse<BusinessPartnerDto>.ReturnError("Neuspješno micanje kontakata za poslovnog partnera.");
                }

                if ((await AddContacts(contactsToAdd)).IsNotSuccess(out ActionResponse<List<ContactPersonDto>> addResponse, out contactsToAdd))
                {
                    return await ActionResponse<BusinessPartnerDto>.ReturnError("Neuspješno dodavanje kontakata za poslovnog partnera.");
                }

                if ((await ModifyContacts(contactsToModify)).IsNotSuccess(out ActionResponse<List<ContactPersonDto>> modifyResponse, out contactsToAdd))
                {
                    return await ActionResponse<BusinessPartnerDto>.ReturnError("Neuspješno ažuriranje kontakata za poslovnog partnera.");
                }

                return await ActionResponse<BusinessPartnerDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dokumenti studenta.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<BusinessPartnerDto>.ReturnError($"Greška prilikom ažuriranja kontakata za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<List<ContactPersonDto>>> RemoveContacts(List<ContactPersonDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<ContactPersonDto>>.ReturnSuccess(null, "Dani uspješno maknuti iz plana.");
                entityDtos.ForEach(async pd =>
                {
                    if ((await RemoveContact(pd))
                        .IsNotSuccess(out ActionResponse<ContactPersonDto> actionResponse))
                    {
                        response = await ActionResponse<List<ContactPersonDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<ContactPersonDto>>.ReturnError("Greška prilikom brisanja kontakata za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<ContactPersonDto>> RemoveContact(ContactPersonDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<ContactPerson>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<ContactPersonDto>.ReturnSuccess(null, "Kontakt za poslovnog partnera uspješno izbrisan obrisan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<ContactPersonDto>.ReturnError("Greška prilikom brisanja kontakta za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<List<ContactPersonDto>>> AddContacts(List<ContactPersonDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<ContactPersonDto>>.ReturnSuccess(null, "Kontakti uspješno dodani.");
                entityDtos.ForEach(async pd =>
                {
                    if ((await AddContact(pd))
                        .IsNotSuccess(out ActionResponse<ContactPersonDto> actionResponse))
                    {
                        response = await ActionResponse<List<ContactPersonDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<ContactPersonDto>>.ReturnError("Greška prilikom brisanja kontakata za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<ContactPersonDto>> AddContact(ContactPersonDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<ContactPersonDto, ContactPerson>(entityDto);
                unitOfWork.GetGenericRepository<ContactPerson>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<ContactPersonDto>.ReturnSuccess(entityDto, "Kontakt za poslovnog partnera uspješno dodan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<ContactPersonDto>.ReturnError("Greška prilikom dodavanja kontakta za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<List<ContactPersonDto>>> ModifyContacts(List<ContactPersonDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<ContactPersonDto>>.ReturnSuccess(null, "Kontakti uspješno ažurirani.");
                entityDtos.ForEach(async pd =>
                {
                    if ((await ModifyContact(pd))
                        .IsNotSuccess(out ActionResponse<ContactPersonDto> actionResponse))
                    {
                        response = await ActionResponse<List<ContactPersonDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<ContactPersonDto>>.ReturnError("Greška prilikom brisanja kontakata za poslovnog partnera.");
            }
        }

        public async Task<ActionResponse<ContactPersonDto>> ModifyContact(ContactPersonDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<ContactPerson>().FindBy(c => c.Id == entityDto.Id.Value);
                mapper.Map<ContactPersonDto, ContactPerson>(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<ContactPerson>().Update(entityToUpdate);
                unitOfWork.Save();
                mapper.Map(entityToUpdate, entityDto);
                return await ActionResponse<ContactPersonDto>.ReturnSuccess(entityDto, "Kontakt za poslovnog partnera uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<ContactPersonDto>.ReturnError("Greška prilikom ažuriranja kontakta za poslovnog partnera.");
            }
        }
    }
}
