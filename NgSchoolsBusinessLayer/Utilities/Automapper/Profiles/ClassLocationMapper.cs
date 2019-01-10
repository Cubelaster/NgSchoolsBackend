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

            CreateMap<ClassLocationsDto, ClassLocations>();
        }
    }
}
