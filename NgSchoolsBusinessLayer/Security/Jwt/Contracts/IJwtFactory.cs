using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Security.Jwt.Contracts
{
    public interface IJwtFactory
    {
        Task<List<Claim>> GetJwtClaims(UserDto user);
        Task<string> GenerateSecurityToken(UserDto user, bool rememberMe);
    }
}