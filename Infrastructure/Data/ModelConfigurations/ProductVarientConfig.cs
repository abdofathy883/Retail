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
    public class ProductVarientConfig : IEntityTypeConfiguration<ProductVarient>
    {
        public void Configure(EntityTypeBuilder<ProductVarient> builder)
        {
            builder.Property(pv => pv.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(pv => pv.ColorId)
                .IsRequired();

            builder.Property(pv => pv.SizeId)
                .IsRequired();

            builder.Property(pv => pv.Price)
                .IsRequired();

            builder.Property(pv => pv.Stock)
                .IsRequired();

            builder.Property(pv => pv.ProductId)
                .IsRequired();

            builder.HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVarients)
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
        }
    }
}
