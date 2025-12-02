using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Core.Data.Models;

public class Order : DatabaseEntity
{
    public string CustomerDocument { get; private set; }
    public string SellerName { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    public Order() { }

    public Order(string customerDocument, string sellerName)
    {
        CustomerDocument = customerDocument;
        SellerName = sellerName;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
}
