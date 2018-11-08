using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Security.Jwt.Contracts
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(List<Claim> userClaims, bool rememberMe = false);
        Task<List<Claim>> GetJWTClaims(UserDto user);
        Task<string> GenerateSecurityToken(UserDto user, bool rememberMe);
    }
}