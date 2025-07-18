using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class WishListItemConfig : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.HasKey(wli => wli.Id);
            builder.Property(wli => wli.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(wli => wli.AddedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(wli => wli.ProductVarient)
                .WithMany()
                .HasForeignKey(wli => wli.ProductVarientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wli => wli.WishList)
                .WithMany(wl => wl.WishListItems)
                .HasForeignKey(wli => wli.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
