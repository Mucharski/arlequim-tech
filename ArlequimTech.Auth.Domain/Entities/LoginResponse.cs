namespace ArlequimTech.Auth.Domain.Entities;

public class LoginResponse
{
    public LoginResponse(string token, DateTime expiresAt, string userName, string userEmail, string role)
    {
        Token = token;
        ExpiresAt = expiresAt;
        UserName = userName;
        UserEmail = userEmail;
        Role = role;
    }

    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string UserName { get; private set; }
    public string UserEmail { get; private set; }
    public string Role { get; private set; }
}
