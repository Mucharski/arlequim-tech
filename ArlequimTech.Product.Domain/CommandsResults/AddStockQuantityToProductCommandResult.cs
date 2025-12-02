using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Product.Domain.CommandsResults;

public class AddStockQuantityToProductCommandResult : GenericCommandResult<Core.Data.Models.Product>
{
    public AddStockQuantityToProductCommandResult(Core.Data.Models.Product data = null, bool success = true, string message = "Quantidade adicionada ao estoque com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
