using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.TotalAmount).IsRequired();
            builder.Property(o => o.ShippingFee).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.Property(o => o.CreatedAt).IsRequired();
            builder.Property(o => o.PaymentProvider).IsRequired();
            builder.Property(o => o.IsDeleted).IsRequired();

            builder.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(a => a.Street).HasMaxLength(200);
                sa.Property(a => a.City).HasMaxLength(100);
                sa.Property(a => a.State).HasMaxLength(100);
                sa.Property(a => a.ZipCode).HasMaxLength(20);
                sa.Property(a => a.Country).HasMaxLength(100);
            });
        }
    }
}
