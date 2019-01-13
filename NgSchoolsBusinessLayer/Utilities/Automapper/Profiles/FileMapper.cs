using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class FileMapper : Profile
    {
        public FileMapper()
        {
            CreateMap<UploadedFile, FileDto>().ReverseMap();
        }
    }
}
