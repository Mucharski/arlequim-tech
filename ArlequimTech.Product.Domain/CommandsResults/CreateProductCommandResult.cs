using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class CreateProductCommandResult : GenericCommandResult<Core.Data.Models.Product>
{
    public CreateProductCommandResult(Core.Data.Models.Product data = null, bool success = true, string message = "Produto criado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
