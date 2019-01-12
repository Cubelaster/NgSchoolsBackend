using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IPlanService
    {
        Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto);
    }
}