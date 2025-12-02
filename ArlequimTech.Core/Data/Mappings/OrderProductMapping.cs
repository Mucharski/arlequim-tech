using ArlequimTech.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArlequimTech.Core.Data.Mappings;

public class OrderProductMapping : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("Order_Product");

        builder.HasKey(op => new { op.OrderId, op.ProductId });

        builder.Property(x => x.OrderId).HasColumnName("OrderId");
        builder.Property(x => x.ProductId).HasColumnName("ProductId");
    }
}
