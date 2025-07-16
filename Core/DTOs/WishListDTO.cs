using Core.Models;

namespace Core.DTOs
{
    public class WishListDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public List<WishListItemDTO>? WishListItems { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
