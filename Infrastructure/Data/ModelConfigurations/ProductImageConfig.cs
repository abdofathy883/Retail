using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pi => pi.IsFeatured)
                .IsRequired();

            builder.Property(pi => pi.ProductId)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ImageUrls)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
