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
                .ForMember(dest => dest.Principal, opt => opt.MapFrom(src => src.Principal));

            CreateMap<InstitutionDto, Institution>()
                .ForMember(dest => dest.Principal, opt => opt.Ignore());
        }
    }
}
