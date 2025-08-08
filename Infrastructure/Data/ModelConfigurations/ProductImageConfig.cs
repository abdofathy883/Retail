using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pi => pi.IsFeatured)
                .IsRequired();

            builder.Property(pi => pi.AltText)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(pi => pi.ProductVariantId)
                .IsRequired();

            builder.HasOne(pi => pi.ProductVariant)
                .WithOne(p => p.VariantImage)
                .HasForeignKey<ProductImage>(pi => pi.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
