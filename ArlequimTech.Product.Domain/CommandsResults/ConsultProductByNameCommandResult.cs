using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class ConsultProductByNameCommandResult : GenericCommandResult<IEnumerable<Core.Data.Models.Product>>
{
    public ConsultProductByNameCommandResult(IEnumerable<Core.Data.Models.Product> data = null, bool success = true, string message = "Produtos encontrados com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
