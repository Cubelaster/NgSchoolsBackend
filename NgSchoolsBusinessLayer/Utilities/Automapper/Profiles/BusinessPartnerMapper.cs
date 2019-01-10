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

            CreateMap<BusinessPartnerDto, BusinessPartner>();
        }
    }
}
