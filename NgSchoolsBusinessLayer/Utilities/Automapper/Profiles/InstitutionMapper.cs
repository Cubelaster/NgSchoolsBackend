using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System.Linq;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class InstitutionMapper: Profile
    {
        public InstitutionMapper()
        {
            CreateMap<Institution, InstitutionDto>()
                .ForMember(dest => dest.GoverningCouncil, opt => opt.MapFrom(src => src.GoverningCouncil))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.InstitutionFiles.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(file => file.File )));

            CreateMap<InstitutionDto, Institution>()
                .ForMember(dest => dest.LogoId, opt => opt.MapFrom(src => src.Logo != null ? src.Logo.Id : null))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.Municipality, opt => opt.Ignore())
                .ForMember(dest => dest.Logo, opt => opt.Ignore())
                .ForMember(dest => dest.Principal, opt => opt.Ignore())
                .ForMember(dest => dest.InstitutionFiles, opt => opt.Ignore())
                .ForMember(dest => dest.GoverningCouncil, opt => opt.Ignore());

            CreateMap<GoverningCouncil, GoverningCouncilDto>()
                .ForMember(dest => dest.GoverningCouncilMembers, opt => opt .MapFrom(src => src.GoverningCouncilMembers.Where(gcm => gcm.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<GoverningCouncilDto, GoverningCouncil>()
                .ForMember(dest => dest.GoverningCouncilMembers, opt => opt.Ignore());

            CreateMap<GoverningCouncilMember, GoverningCouncilMemberDto>();

            CreateMap<GoverningCouncilMemberDto, GoverningCouncilMember>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<InstitutionFile, InstitutionFileDto>().ReverseMap();
        }
    }
}
