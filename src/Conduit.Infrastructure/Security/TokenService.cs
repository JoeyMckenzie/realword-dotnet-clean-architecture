namespace Conduit.Infrastructure.Security
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using Core.Exceptions;
    using Core.Infrastructure;
    using Domain.Entities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Shared;

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTime _dateTime;

        public TokenService(IConfiguration configuration, IDateTime dateTime)
        {
            _configuration = configuration;
            _dateTime = dateTime;
        }

        public string CreateToken(ConduitUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["ConduitJwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = BuildUserBasedClaims(user),
                Issuer = _configuration["ConduitTokenIssuer"],
                Audience = _configuration["ConduitTokenAudience"],
                IssuedAt = _dateTime.Now,
                Expires = _dateTime.Now.AddMinutes(30),
                NotBefore = _dateTime.Now.AddMinutes(29),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            string tokenString;

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                throw new ConduitApiException($"Could not generate security token for user request [{user.Id}] ({user.Email}), reason: {e.Message}", HttpStatusCode.InternalServerError);
            }

            if (string.IsNullOrWhiteSpace(tokenString))
            {
                throw new ConduitApiException($"Security was not generated properly for user [{user.Id}] ({user.Email}), please try again", HttpStatusCode.BadRequest);
            }

            return tokenString;
        }

        public string CreateAnonymousToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["ConduitJwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Anonymous, "anonymous"),
                    new Claim(ClaimTypes.Name, "ANONYMOUS_USER"),
                }),
                Issuer = _configuration["ConduitTokenIssuer"],
                Audience = _configuration["ConduitTokenAudience"],
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private static ClaimsIdentity BuildUserBasedClaims(ConduitUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.UserName),
                new Claim("username", user.UserName),
            };

            return new ClaimsIdentity(claims);
        }
    }
}