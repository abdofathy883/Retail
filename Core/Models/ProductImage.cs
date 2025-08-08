namespace Core.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = default!;
        public bool IsFeatured { get; set; }
        public string AltText { get; set; }

        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = default!;
    }
}
