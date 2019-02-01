using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class LocationMapper : Profile
    {
        public LocationMapper()
        {
            CreateMap<Country, CountryDto>();

            CreateMap<CountryDto, Country>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Regions, opt => opt.Ignore());

            CreateMap<Region, RegionDto>();

            CreateMap<RegionDto, Region>()
                .ForMember(dest => dest.Cities, opt => opt.Ignore());

            CreateMap<City, CityDto>().ReverseMap();
        }
    }
}
