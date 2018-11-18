using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IFileUploadService
    {
        Task<ActionResponse<string>> Upload(FileUploadRequest fileUploadRequest);
    }
}