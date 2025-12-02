using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class DeleteProductCommandResult : GenericCommandResult<bool>
{
    public DeleteProductCommandResult(bool data = true, bool success = true, string message = "Produto excluído com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
