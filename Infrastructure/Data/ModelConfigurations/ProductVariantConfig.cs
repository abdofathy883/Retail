using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class ProductVariantConfig : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.Property(pv => pv.Id)
                .UseIdentityColumn(1, 1)
                .ValueGeneratedOnAdd();

            builder.Property(pv => pv.OriginalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(pv => pv.SalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            builder.Property(pv => pv.WholesalePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(pv => pv.Stock)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pv => pv.SKU)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pv => pv.Barcode)
                .HasMaxLength(100);

            builder.Property(pv => pv.NuOfPurchases)
                .HasDefaultValue(0);

            builder.Property(pv => pv.NuOfPutInCart)
                .HasDefaultValue(0);

            builder.Property(pv => pv.NuOfPutInWishList)
                .HasDefaultValue(0);

            builder.Property(pv => pv.ColorId)
                .IsRequired();

            builder.Property(pv => pv.SizeId)
                .IsRequired();

            builder.HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVariants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pv => pv.Color)
                .WithMany()
                .HasForeignKey(pv => pv.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Size)
                .WithMany()
                .HasForeignKey(pv => pv.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.VariantImage)
                .WithOne(pvi => pvi.ProductVariant)
                .HasForeignKey<ProductImage>(pvi => pvi.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
