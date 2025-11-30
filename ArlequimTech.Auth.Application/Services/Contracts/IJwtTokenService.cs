using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Auth.Application.Services.Contracts;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
