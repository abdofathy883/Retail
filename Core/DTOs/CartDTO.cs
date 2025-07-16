using Core.Models;

namespace Core.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CartToken { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
        public DateTime LastUpdatedAt { get; set; }
    }
}
