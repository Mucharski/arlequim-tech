using ArlequimTech.Auth.Domain.Repositories;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Auth.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }
}