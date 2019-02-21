using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(sf => new FileDto { Id = sf.File.Id, FileName = sf.File.FileName })));

            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo != null ? src.Photo.Id : null))
                .ForMember(dest => dest.AddressCity, opt => opt.Ignore())
                .ForMember(dest => dest.AddressCountry, opt => opt.Ignore())
                .ForMember(dest => dest.AddressRegion, opt => opt.Ignore())
                .ForMember(dest => dest.StudentsInGroups, opt => opt.Ignore())
                .ForMember(dest => dest.CountryOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.CityOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.RegionOfBirth, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.Employer, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore());

            CreateMap<StudentFiles, StudentFileDto>().ReverseMap();
        }
    }
}
