using Core.DTOs;
using Core.DTOs.WishListDTOs;

namespace Core.Interfaces
{
    public interface IWishListService
    {
        Task<WishListDTO> AddToWishlistAsync(AddToWishListDTO wishListDTO);
        Task<WishListDTO> RemoveFromWishlistAsync(string customerId, int wishlistItemId);
        Task<WishListDTO> ClearWishlistAsync(string customerId);
        Task<WishListDTO> GetWishlistByUserIdAsync(string customerId);
        Task<bool> IsInWishlistAsync(string customerId, int productVarientId);
    }
}
