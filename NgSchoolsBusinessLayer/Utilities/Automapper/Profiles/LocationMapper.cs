using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class LocationMapper : Profile
    {
        public LocationMapper()
        {
            CreateMap<Country, CountryDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Alpha2Code))
                .PreserveReferences();

            CreateMap<CountryDto, Country>()
                .ForMember(dest => dest.Alpha2Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Regions, opt => opt.Ignore())
                .PreserveReferences();

            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Name))
                .PreserveReferences();

            CreateMap<RegionDto, Region>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Region))
                .PreserveReferences();

            CreateMap<City, CityDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .PreserveReferences();

            CreateMap<CityDto, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City))
                .PreserveReferences();

            CreateMap<MunicipalityDto, Municipality>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .PreserveReferences();

            CreateMap<Municipality, MunicipalityDto>()
                .PreserveReferences();
        }
    }
}
