﻿using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IStudentService
    {
        Task<ActionResponse<List<StudentDto>>> GetAllForCache();
        Task<ActionResponse<StudentDto>> GetById(int id);
        Task<ActionResponse<StudentDto>> GetByOib(string oib);
        Task<ActionResponse<List<StudentDto>>> GetAll();
        Task<ActionResponse<int>> GetTotalNumber();
        Task<ActionResponse<List<StudentDto>>> GetTenNewest();
        Task<ActionResponse<PagedResult<StudentDto>>> GetBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<StudentDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<StudentDto>> Insert(StudentDto entityDto);
        Task<ActionResponse<StudentDto>> Update(StudentDto entityDto);
        Task<ActionResponse<StudentDto>> Delete(int id);
        Task<ActionResponse<StudentDto>> UpdateEnrollmentDate(StudentDto student);
        Task<ActionResponse<StudentEducationProgramsPrintModel>> GetStudentsEducationPrograms(int studentId);
    }
}