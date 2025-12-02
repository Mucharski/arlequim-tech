using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class ListProductsCommandResult : GenericCommandResult<IEnumerable<Core.Data.Models.Product>>
{
    public ListProductsCommandResult(IEnumerable<Core.Data.Models.Product> data = null, bool success = true, string message = "Produtos listados com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
