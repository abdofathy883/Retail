namespace Core.DTOs
{
    public class AddToCartDTO
    {
        public Guid ProductId { get; set; }
        public int? ProductVariantId { get; set; } // null if simple product
        public int Quantity { get; set; }
    }
}
