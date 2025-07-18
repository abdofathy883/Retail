using Core.DTOs;

namespace Core.Interfaces
{
    public interface IWishListService
    {
        Task<WishListDTO> AddToWishlistAsync(string customerId, int productVarientId);
        Task<WishListDTO> RemoveFromWishlistAsync(string customerId, int wishlistItemId);
        Task<WishListDTO> ClearWishlistAsync(string customerId);
        Task<WishListDTO> GetWishlistByUserIdAsync(string customerId);
        Task<bool> IsInWishlistAsync(string customerId, int productVarientId);
    }
}
