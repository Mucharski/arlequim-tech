using ArlequimTech.Auth.Domain.Entities;
using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Auth.Domain.CommandsResults;

public class CreateUserCommandResult : GenericCommandResult<CreateUserResponse>
{
    public CreateUserCommandResult(CreateUserResponse data = null, bool success = true, string message = "Usuário criado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}