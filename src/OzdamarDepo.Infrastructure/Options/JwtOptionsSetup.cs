using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OzdamarDepo.Infrastructure.Options;
using System.Security.Claims;
using System.Text;

public sealed class JwtOptionsSetup(
 IOptions<JwtOptions> jwtOptions) : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;

        options.TokenValidationParameters.ValidIssuer = jwtOptions.Value.Issuer;
        options.TokenValidationParameters.ValidAudience = jwtOptions.Value.Audience;
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));

        options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier; // ✅ EKLE BUNU
    }
}
