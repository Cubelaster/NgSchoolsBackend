using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IIssuedPrintService
    {
        Task<ActionResponse<IssuedPrintDto>> GetById(int id);
        Task<ActionResponse<List<IssuedPrintDto>>> GetAll();
        Task<ActionResponse<List<IssuedPrintDto>>> GetForStudentAndProgram(IssuedPrintDto entityDto);
        Task<ActionResponse<int>> GetForStudentAndProgramTotalDuplicates(IssuedPrintDto entityDto);
        Task<ActionResponse<Dictionary<DateTime, int>>> GetForCurrentYear(SimpleRequestBase request);
        Task<ActionResponse<IssuedPrintDto>> Insert(IssuedPrintDto entityDto);
        Task<ActionResponse<IssuedPrintDto>> Update(IssuedPrintDto entityDto);
        Task<ActionResponse<IssuedPrintDto>> Increment(IssuedPrintDto entityDto);
        Task<ActionResponse<IssuedPrintDto>> Delete(int id);
    }
}
