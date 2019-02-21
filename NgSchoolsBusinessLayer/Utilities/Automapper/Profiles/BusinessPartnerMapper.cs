using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class BusinessPartnerMapper : Profile
    {
        public BusinessPartnerMapper()
        {
            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dest => dest.BusinessPartnerContacts, opt => opt.MapFrom(src => src.BusinessPartnerContacts.Where(bpc => bpc.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<BusinessPartnerDto, BusinessPartner>()
                .ForMember(dest => dest.BusinessPartnerContacts, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());

            CreateMap<ContactPerson, ContactPersonDto>().ReverseMap();
        }
    }
}
