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
    public class WishListConfig : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.HasKey(wl => wl.Id);

            builder.Property(wl => wl.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(wl => wl.UserId)
                .IsRequired();

            builder.Property(wl => wl.WishListItems)
                .IsRequired();

            builder.Property(wl => wl.LastUpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnUpdate();

            builder.HasOne(wl => wl.User)
                .WithMany()
                .HasForeignKey(wl => wl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(wl => wl.WishListItems)
                .WithOne(wli => wli.WishList)
                .HasForeignKey(wli => wli.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
