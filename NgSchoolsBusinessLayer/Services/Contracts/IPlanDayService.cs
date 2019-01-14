using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IPlanDayService
    {
        Task<ActionResponse<PlanDayDto>> Insert(PlanDayDto entityDto);
        Task<ActionResponse<List<PlanDayDto>>> InsertPlanDays(List<PlanDayDto> entityDtos);
    }
}