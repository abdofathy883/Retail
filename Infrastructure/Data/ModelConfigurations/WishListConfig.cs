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
            builder.Property(wl => wl.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(wl => wl.UserId)
                .IsRequired();

            builder.Property(wl => wl.WishListItems)
                .IsRequired();


            builder.HasOne(wl => wl.User)
                .WithOne(u => u.WishList)
                .HasForeignKey<WishList>(wl => wl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(wl => wl.WishListItems)
                .WithOne(wli => wli.WishList)
                .HasForeignKey(wli => wli.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
