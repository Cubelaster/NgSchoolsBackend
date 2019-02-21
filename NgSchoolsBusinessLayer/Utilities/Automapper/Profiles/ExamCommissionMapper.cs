using AutoMapper;
using System.Linq;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Enums;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ExamCommissionMapper : Profile
    {
        public ExamCommissionMapper()
        {
            CreateMap<ExamCommission, ExamCommissionDto>()
                .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src => src.UserExamCommissions.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(uex => uex.UserId)))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.UserExamCommissions.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(uex => $"{uex.User.UserDetails.FirstName} {uex.User.UserDetails.LastName}").ToList()))
                .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => string.Join(",", src.UserExamCommissions.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(uex => $"{uex.User.UserDetails.FirstName} {uex.User.UserDetails.LastName}").ToList())));

            CreateMap<ExamCommissionDto, ExamCommission>();
        }
    }
}
