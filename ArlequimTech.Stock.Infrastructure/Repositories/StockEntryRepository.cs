using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Stock.Domain.Repositories;

namespace ArlequimTech.Stock.Infrastructure.Repositories;

public class StockEntryRepository : BaseRepository<StockEntry>, IStockEntryRepository
{
    public StockEntryRepository(Context context) : base(context)
    {
    }
}
