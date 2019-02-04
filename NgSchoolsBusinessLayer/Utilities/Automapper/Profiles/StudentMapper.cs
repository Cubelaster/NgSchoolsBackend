using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files.Select(sf => new FileDto { Id = sf.File.Id, FileName = sf.File.FileName })));

            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo != null ? src.Photo.Id : null))
                .ForMember(dest => dest.AddressCity, opt => opt.Ignore())
                .ForMember(dest => dest.AddressCountry, opt => opt.Ignore())
                .ForMember(dest => dest.AddressRegion, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerRegion, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerCity, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerCountry, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroups, opt => opt.Ignore())
                .ForMember(dest => dest.CountryOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.CityOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.RegionOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.Files, opt => opt.Ignore());

            CreateMap<StudentFiles, StudentFileDto>().ReverseMap();
        }
    }
}
