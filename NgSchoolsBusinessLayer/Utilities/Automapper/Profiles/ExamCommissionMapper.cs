using AutoMapper;
using System.Linq;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.Automapper.Profiles
{
    public class ExamCommissionMapper : Profile
    {
        public ExamCommissionMapper()
        {
            CreateMap<ExamCommission, ExamCommissionDto>()
                .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src => src.UserExamCommissions.Select(uex => uex.User.Id)))
                .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.UserExamCommissions.Select(uex => $"{uex.User.UserDetails.FirstName}  {uex.User.UserDetails.LastName}")));

            CreateMap<ExamCommissionDto, ExamCommission>();
        }
    }
}
