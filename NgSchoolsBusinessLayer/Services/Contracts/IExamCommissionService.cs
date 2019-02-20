using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IExamCommissionService
    {
        Task<ActionResponse<ExamCommissionDto>> Delete(int id);
        Task<ActionResponse<List<ExamCommissionDto>>> GetAll();
        Task<ActionResponse<PagedResult<ExamCommissionDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<ExamCommissionDto>> GetById(int id);
        Task<ActionResponse<ExamCommissionDto>> Insert(ExamCommissionDto entityDto);
        Task<ActionResponse<ExamCommissionDto>> Update(ExamCommissionDto entityDto);
        Task<ActionResponse<int>> GetTotalNumber();
    }
}