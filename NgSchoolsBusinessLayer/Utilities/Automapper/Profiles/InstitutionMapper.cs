using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class InstitutionMapper: Profile
    {
        public InstitutionMapper()
        {
            CreateMap<Institution, InstitutionDto>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.Principal, opt => opt.MapFrom(src => src.Principal));

            CreateMap<InstitutionDto, Institution>()
                .ForMember(dest => dest.LogoId, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Id : null))
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.Principal, opt => opt.Ignore());
        }
    }
}
