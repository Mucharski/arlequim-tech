using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data;
using ArlequimTech.Product.Domain.Repositories;

namespace ArlequimTech.Product.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Core.Data.Models.Product>, IProductRepository
{
    public ProductRepository(Context context) : base(context)
    {
    }
}
