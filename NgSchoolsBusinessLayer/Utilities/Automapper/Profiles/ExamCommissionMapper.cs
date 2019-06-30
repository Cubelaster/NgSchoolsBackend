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
                .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.UserExamCommissions.Where(a => a.Status == DatabaseEntityStatusEnum.Active).Select(uex => uex.User).ToList()))
                .ForMember(dest => dest.CommissionMembers, opt => opt.MapFrom(src => src.UserExamCommissions.Where(a => a.Status == DatabaseEntityStatusEnum.Active)));

            CreateMap<ExamCommissionDto, ExamCommission>();
        }
    }
}
