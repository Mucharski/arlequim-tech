using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArlequimTech.Auth.Application.Services.Contracts;
using ArlequimTech.Core.Configuration;
using ArlequimTech.Core.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ArlequimTech.Auth.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
