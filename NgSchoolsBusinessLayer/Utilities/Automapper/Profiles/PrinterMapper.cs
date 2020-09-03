using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class PrinterMapper : Profile
    {
        public PrinterMapper()
        {
            CreateMap<Printer, PrinterDto>().ReverseMap();
        }
    }
}
