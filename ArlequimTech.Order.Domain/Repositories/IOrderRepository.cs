using ArlequimTech.Core.BaseClasses.Interfaces;

namespace ArlequimTech.Order.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<Core.Data.Models.Order>
{
    Task<Core.Data.Models.Product?> GetProductByIdAsync(Guid productId);
    Task<List<Core.Data.Models.Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds);
    Task SaveChangesAsync();
}
