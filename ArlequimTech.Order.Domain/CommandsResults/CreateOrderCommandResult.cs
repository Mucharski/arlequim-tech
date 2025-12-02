using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Order.Domain.CommandsResults;

public class CreateOrderCommandResult : GenericCommandResult<Core.Data.Models.Order>
{
    public CreateOrderCommandResult(Core.Data.Models.Order data = null, bool success = true, string message = "Pedido criado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
