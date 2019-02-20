using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IStudentRegisterService
    {
        Task<ActionResponse<StudentRegisterDto>> GetById(int id);
        Task<ActionResponse<List<StudentRegisterDto>>> GetAll();
        Task<ActionResponse<List<StudentRegisterDto>>> GetAllNotFull();
        Task<ActionResponse<List<StudentRegisterDto>>> GetAllForCache();
        Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest);

        Task<ActionResponse<StudentRegisterEntryDto>> GetEntryById(int id);
        Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntries();
        Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntriesForCache();
        Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesByBookIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<StudentRegisterEntryDto>> InsertEntry(StudentRegisterEntryInsertRequest request);
        Task<ActionResponse<StudentRegisterEntryDto>> UpdateEntry(StudentRegisterEntryInsertRequest request);
        Task<ActionResponse<int>> GetTotalNumber();
    }
}