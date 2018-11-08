using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Responses;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IAuthService
    {
        Task<ActionResponse<LoginResponse>> Login(LoginRequest loginRequest);
    }
}
