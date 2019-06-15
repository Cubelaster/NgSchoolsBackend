using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IThemeService
    {
        #region Readers

        Task<ActionResponse<ThemeDto>> GetById(int id);
        Task<ActionResponse<List<ThemeDto>>> GetAll();
        Task<ActionResponse<List<ThemeDto>>> GetAllBySubjectId(int subjectId);
        Task<ActionResponse<PagedResult<ThemeDto>>> GetAllPaged(BasePagedRequest pagedRequest);

        #endregion Readers

        Task<ActionResponse<ThemeDto>> Insert(ThemeDto entityDto);
        Task<ActionResponse<ThemeDto>> Update(ThemeDto entityDto);
        Task<ActionResponse<ThemeDto>> Delete(int id);
        Task<ActionResponse<List<ThemeDto>>> InsertThemes(List<ThemeDto> entityDtos);
        Task<ActionResponse<int>> GetTotalNumber();

        #region Unused

        //Task<ActionResponse<List<ThemeDto>>> ModifyThemesForEducationProgram(List<ThemeDto> themes);

        #endregion Unused
    }
}