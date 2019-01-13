﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IStudentGroupService
    {
        Task<ActionResponse<StudentGroupDto>> Delete(int id);
        Task<ActionResponse<List<StudentGroupDto>>> GetAll();
        Task<ActionResponse<PagedResult<StudentGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<StudentGroupDto>> GetById(int id);
        Task<ActionResponse<StudentGroupDto>> Insert(StudentGroupDto entityDto);
        Task<ActionResponse<StudentGroupDto>> Update(StudentGroupDto entityDto);
    }
}