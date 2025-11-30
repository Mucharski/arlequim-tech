using ArlequimTech.Auth.Domain.Entities;
using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Auth.Domain.CommandsResults;

public class LoginCommandResult : GenericCommandResult<LoginResponse>
{
    public LoginCommandResult(LoginResponse data = null, bool success = true, string message = "Login realizado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
