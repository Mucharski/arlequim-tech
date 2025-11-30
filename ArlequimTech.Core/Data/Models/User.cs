using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Enums;

namespace ArlequimTech.Core.Data.Models;

public class User : DatabaseEntity
{
    public User()
    {
        
    }
    
    public User(string name, string email, string password, UserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }
    
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
