namespace Core.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = default!;
        public bool IsFeatured { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
