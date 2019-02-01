using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class BusinessPartnerMapper : Profile
    {
        public BusinessPartnerMapper()
        {
            CreateMap<BusinessPartner, BusinessPartnerDto>();

            CreateMap<BusinessPartnerDto, BusinessPartner>()
                .ForMember(dest => dest.BusinessPartnerContacts, opt => opt.Ignore());

            CreateMap<ContactPerson, ContactPersonDto>().ReverseMap();
        }
    }
}
