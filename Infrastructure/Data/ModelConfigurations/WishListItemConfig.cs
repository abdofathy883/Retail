using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class WishListItemConfig : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.ToTable("WishListItems");
            builder.HasKey(wli => wli.Id);

            builder.Property(wli => wli.ProductId).IsRequired();
            builder.Property(wli => wli.WishListId).IsRequired();
            builder.Property(wli => wli.CreatedAt).IsRequired();
            builder.Property(wli => wli.IsDeleted).IsRequired();

            builder.HasOne(wli => wli.Product)
                .WithMany()
                .HasForeignKey(wli => wli.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wli => wli.WishList)
                .WithMany(wl => wl.WishListItems)
                .HasForeignKey(wli => wli.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
