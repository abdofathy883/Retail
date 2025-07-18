using Core.Models;

namespace Core.DTOs
{
    public class WishListItemDTO
    {
        public int Id { get; set; }
        public int productVarientId { get; set; }
        public int WishListId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
