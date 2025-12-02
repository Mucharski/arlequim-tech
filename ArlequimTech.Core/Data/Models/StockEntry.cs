using ArlequimTech.Core.BaseClasses;

namespace ArlequimTech.Core.Data.Models;

public class StockEntry : DatabaseEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string InvoiceNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Product Product { get; private set; }

    public StockEntry() { }

    public StockEntry(Guid productId, int quantity, string invoiceNumber)
    {
        ProductId = productId;
        Quantity = quantity;
        InvoiceNumber = invoiceNumber;
        CreatedAt = DateTime.UtcNow;
    }
}
