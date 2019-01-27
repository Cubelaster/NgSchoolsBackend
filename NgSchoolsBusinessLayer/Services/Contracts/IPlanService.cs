using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IPlanService
    {
        Task<ActionResponse<PlanDto>> GetById(int id);
        Task<ActionResponse<List<PlanDto>>> GetAll();
        Task<ActionResponse<PagedResult<PlanDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PlanDto>> GetByEducationProgramId(int id);
        Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto);
        Task<ActionResponse<PlanDto>> Update(PlanDto entityDto);
        Task<ActionResponse<PlanDto>> Delete(int id);

        #region Unused

        //Task<ActionResponse<EducationProgramDto>> InsertPlanForEducationProgram(EducationProgramDto completeEduProgram);
        //Task<ActionResponse<EducationProgramDto>> UpdatePlanForEducationProgram(EducationProgramDto completeEduProgram);

        #endregion Unused
    }
}