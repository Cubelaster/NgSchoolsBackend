using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IDiaryService
    {
        Task<ActionResponse<List<DiaryStudentGroupDto>>> AddStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos);
        Task<ActionResponse<DiaryStudentGroupDto>> DeleteDiaryStudentGroup(DiaryStudentGroupDto entityDto);
        Task<ActionResponse<DiaryDto>> Insert(DiaryDto entityDto);
        Task<ActionResponse<DiaryStudentGroupDto>> InsertDiaryStudentGroup(DiaryStudentGroupDto entityDto);
        Task<ActionResponse<List<DiaryStudentGroupDto>>> RemoveStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos);
    }
}