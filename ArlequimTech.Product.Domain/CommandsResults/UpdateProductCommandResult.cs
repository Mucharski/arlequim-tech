using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class UpdateProductCommandResult : GenericCommandResult<Core.Data.Models.Product>
{
    public UpdateProductCommandResult(Core.Data.Models.Product data = null, bool success = true, string message = "Produto atualizado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
