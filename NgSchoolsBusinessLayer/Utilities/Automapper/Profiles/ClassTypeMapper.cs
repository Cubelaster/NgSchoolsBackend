using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ClassTypeMapper : Profile
    {
        public ClassTypeMapper()
        {
            CreateMap<ClassType, ClassTypeDto>();

            CreateMap<ClassTypeDto, ClassType>();
        }
    }
}
