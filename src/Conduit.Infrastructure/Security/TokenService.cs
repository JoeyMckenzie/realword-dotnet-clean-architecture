namespace Conduit.Infrastructure.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
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
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["Conduit:JwtSecret"]);
            var issuer = _configuration["Conduit:Issuer"];
            var audience = _configuration["Conduit:Audience"];

            /*
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = BuildUserBasedClaims(user),
                Issuer = _configuration["Conduit:TokenIssuer"],
                Audience = _configuration["Conduit:TokenAudience"],
                IssuedAt = _dateTime.Now,
                Expires = _dateTime.Now.AddMinutes(60),
                NotBefore = _dateTime.Now.AddMinutes(45),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            */

            // string tokenString;
            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: GetDefaultClaims(user, issuer, audience),
                notBefore: _dateTime.Now,
                expires: _dateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature));

            /*
            try
            {
                var token = tokenHandler.CreateToken(securityToken);
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
            */

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
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

        private static IEnumerable<Claim> GetDefaultClaims(ConduitUser user, string issuer, string audience)
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.UserName)
            };
        }
    }
}