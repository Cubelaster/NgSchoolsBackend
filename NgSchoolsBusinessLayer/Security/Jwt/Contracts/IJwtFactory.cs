using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Responses;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Security.Jwt.Contracts
{
    public interface IJwtFactory
    {
        Task<List<Claim>> GetJwtClaims(UserDto user);
        Task<GenerateJwtTokenResponse> GenerateSecurityToken(UserDto user, bool rememberMe);
    }
}