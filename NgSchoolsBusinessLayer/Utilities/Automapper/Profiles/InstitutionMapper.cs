using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class InstitutionMapper: Profile
    {
        public InstitutionMapper()
        {
            CreateMap<Institution, InstitutionDto>();

            CreateMap<InstitutionDto, Institution>()
                .ForMember(dest => dest.LogoId, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Id : null))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.Principal, opt => opt.Ignore());
        }
    }
}
