﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OzdamarDepo.Application.Services;
using OzdamarDepo.Domain.Users;
using OzdamarDepo.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OzdamarDepo.Infrastructure.Services
{
    public sealed class JwtProvider(
        IOptions<JwtOptions> options) : IJwtProvider
    {
        public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
        {

            List<Claim> claims = new()
            {
                new Claim("user-id",user.Id.ToString())
            };
            var expires = DateTime.UtcNow.AddMonths(1);

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken securityToken = new(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: signingCredentials);
            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(securityToken);
            return Task.FromResult(token);
        }
    }
}
