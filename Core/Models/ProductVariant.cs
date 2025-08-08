using Core.Interfaces;

namespace Core.Models
{
    public class ProductVariant: IDeletable, IProduct
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int ColorId { get; set; }
        public Color Color { get; set; } = default!;
        public int SizeId { get; set; }
        public Size Size { get; set; } = default!;
        public int VariantImageId { get; set; }
        public ProductImage VariantImage { get; set; } = default!;
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }

        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFeatured { get; set; }
    }
}
