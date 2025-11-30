namespace ArlequimTech.Core.Data.Models;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<StockEntry> StockEntries { get; private set; }
    public ICollection<Order> Orders { get; private set; }
}
