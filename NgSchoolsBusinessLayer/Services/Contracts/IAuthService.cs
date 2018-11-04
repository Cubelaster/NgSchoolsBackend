using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IAuthService
    {
        Task<ActionResponse<LoginRequest>> Login(LoginRequest loginRequest);
    }
}
