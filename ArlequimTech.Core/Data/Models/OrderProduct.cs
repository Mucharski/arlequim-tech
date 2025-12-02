namespace ArlequimTech.Core.Data.Models;

public class OrderProduct
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }

    public OrderProduct() { }

    public OrderProduct(Guid orderId, Guid productId)
    {
        OrderId = orderId;
        ProductId = productId;
    }
}
