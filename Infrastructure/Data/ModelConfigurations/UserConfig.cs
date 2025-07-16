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
    public class UserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.IsDeleted).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();

            builder.HasOne(u => u.Cart)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.WishList)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
