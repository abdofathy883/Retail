namespace Core.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProductVariantDTO>? ProductVariants { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
