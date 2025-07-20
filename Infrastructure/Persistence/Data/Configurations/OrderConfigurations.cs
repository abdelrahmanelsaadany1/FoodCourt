using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Remove the Customer relationship configuration entirely
        // Keep only same-database relationships

        builder
            .HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(o => o.Status)
            .HasConversion<string>();

        builder
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId);
    }
}