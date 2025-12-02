using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.Data;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Order.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArlequimTech.Order.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Core.Data.Models.Order>, IOrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context) : base(context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds)
    {
        return await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
