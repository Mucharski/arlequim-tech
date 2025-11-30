using ArlequimTech.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArlequimTech.Core.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.CustomerDocument)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(o => o.SellerName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity<OrderProduct>(
                "Order_Product",
                j => j.HasOne<Product>().WithMany().HasForeignKey(op => op.ProductId),
                j => j.HasOne<Order>().WithMany().HasForeignKey(op => op.OrderId),
                j => j.HasKey(op => new { op.OrderId, op.ProductId })
            );
    }
}
