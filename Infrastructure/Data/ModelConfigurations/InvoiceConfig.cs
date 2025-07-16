using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber).IsRequired();
            builder.Property(i => i.ShippingFee).IsRequired();
            builder.Property(i => i.Total).IsRequired();
            builder.Property(i => i.PaymentMethod).IsRequired();
            builder.Property(i => i.CreatedAt).IsRequired();
            builder.Property(i => i.IsDeleted).IsRequired();

            builder.HasOne(i => i.Order)
                .WithOne()
                .HasForeignKey<Invoice>(i => i.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(i => i.ShippingAddress, sa =>
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
