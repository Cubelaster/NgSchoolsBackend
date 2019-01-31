using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class LocationMapper : Profile
    {
        public LocationMapper()
        {
            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<Region, RegionDto>().ReverseMap();

            CreateMap<City, CityDto>().ReverseMap();
        }
    }
}
