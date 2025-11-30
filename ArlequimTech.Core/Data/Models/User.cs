using ArlequimTech.Core.Data.Enums;

namespace ArlequimTech.Core.Data.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
