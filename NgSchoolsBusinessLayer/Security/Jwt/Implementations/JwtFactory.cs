using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Security.Jwt.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Security.Jwt.Implementations
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.jwtIssuerOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtIssuerOptions);
        }

        public async Task<string> GenerateSecurityToken(UserDto user, bool rememberMe)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(user.Claims),
                SigningCredentials = jwtIssuerOptions.SigningCredentials,
                Audience = jwtIssuerOptions.Audience,
                //EncryptingCredentials = new EncryptingCredentials(jwtIssuerOptions.SecurityKey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256),
                Issuer = jwtIssuerOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow
            };

            if (!rememberMe)
            {
                tokenDescriptor.Expires = DateTime.UtcNow.AddHours(1);
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return await Task.FromResult(tokenString);
        }

        public async Task<List<Claim>> GetJwtClaims(UserDto user)
        {
            var claims = new List<Claim>()
            {
                 new Claim(JwtClaimIdentifiersEnum.Id.ToString(), user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, await jwtIssuerOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
            };

            return await Task.FromResult(claims);
        }

        #region Helpers

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        #endregion Heplers
    }
}
