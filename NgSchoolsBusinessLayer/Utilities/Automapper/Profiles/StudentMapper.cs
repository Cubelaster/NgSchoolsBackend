using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));

            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo != null ? src.Photo.Id : null))
                .ForMember(dest => dest.Photo, opt => opt.Ignore());
        }
    }
}
