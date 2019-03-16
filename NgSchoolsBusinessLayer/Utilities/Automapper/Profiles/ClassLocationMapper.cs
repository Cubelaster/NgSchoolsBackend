using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ClassLocationMapper : Profile
    {
        public ClassLocationMapper()
        {
            CreateMap<ClassLocations, ClassLocationsDto>();

            CreateMap<ClassLocationsDto, ClassLocations>()
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.Municipality, opt => opt.Ignore());
        }
    }
}
