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
                .ForMember(dest => dest.Regions, opt => opt.Ignore());

            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Name));

            CreateMap<RegionDto, Region>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Region));

            CreateMap<City, CityDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name));

            CreateMap<CityDto, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.City));

            CreateMap<MunicipalityDto, Municipality>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore());

            CreateMap<Municipality, MunicipalityDto>();
        }
    }
}
