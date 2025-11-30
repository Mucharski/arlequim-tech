namespace ArlequimTech.Core.Data.Models;

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerDocument { get; private set; }
    public string SellerName { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<Product> Products { get; private set; }
}
