using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Core.Data.Models;

public class Product : DatabaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ICollection<StockEntry> StockEntries { get; private set; }
    public ICollection<Order> Orders { get; private set; }

    public Product() { }

    public Product(string name, string description, decimal price, int quantityInStock)
    {
        Name = name;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
    }

    public void Update(string? name, string? description, decimal? price)
    {
        Name = name ?? this.Name;
        Description = description ?? this.Description;
        Price = price ?? this.Price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddQuantity(int quantity)
    {
        QuantityInStock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
