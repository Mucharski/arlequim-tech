using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data.Models;

namespace ArlequimTech.Stock.Domain.CommandsResults;

public class AddStockCommandResult : GenericCommandResult<StockEntry>
{
    public AddStockCommandResult(StockEntry data = null, bool success = true, string message = "Estoque adicionado com sucesso!", IEnumerable<string> errors = null) : base(data, success, message, errors)
    {
    }
}
