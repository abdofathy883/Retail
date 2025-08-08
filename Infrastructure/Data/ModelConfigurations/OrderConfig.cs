using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ShippingFee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.PaidAt)
                .IsRequired(false);

            builder.Property(o => o.DeliveredAt)
                .IsRequired(false);

            builder.Property(o => o.PaymentProvider)
                .IsRequired();

            builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder.OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(a => a.StreetName).HasMaxLength(200);
                sa.Property(a => a.City).HasMaxLength(100);
                sa.Property(a => a.BuildingBumber).HasMaxLength(20);
                sa.Property(a => a.PostalCode).HasMaxLength(20);
                sa.Property(a => a.Country).HasMaxLength(100);
            });
        }
    }
}
