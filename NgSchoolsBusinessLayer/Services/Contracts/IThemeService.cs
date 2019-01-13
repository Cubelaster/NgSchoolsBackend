using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IThemeService
    {
        Task<ActionResponse<ThemeDto>> Insert(ThemeDto entityDto);
        Task<ActionResponse<List<ThemeDto>>> ModifyThemesForEducationProgram(List<ThemeDto> themes);
        Task<ActionResponse<ThemeDto>> Update(ThemeDto entityDto);
    }
}