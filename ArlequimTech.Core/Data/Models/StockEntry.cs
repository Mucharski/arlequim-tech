namespace ArlequimTech.Core.Data.Models;

public class StockEntry
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string InvoiceNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Product Product { get; private set; }
}
