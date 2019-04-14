using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IPdfGeneratorService
    {
        Task<ActionResponse<FileDto>> GeneratePdf(string fileName);
    }
}
