using Core.DTOs;
using Core.Models.ValueObjects;

namespace Core.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> AddToCartAsync(CartOwner cartOwner, AddToCartDTO newCartItem);
        Task<CartDTO> UpdateCartItemQuantityAsync(CartOwner cartOwner, UpdateCartDTO updateCart);
        Task<CartDTO> RemoveFromCartAsync(CartOwner cartOwner, int cartItemId);
        Task<CartDTO> ClearCartAsync(CartOwner cartOwner);
        Task<CartDTO> GetCartAsync(CartOwner cartOwner);
        Task<CartDTO> MergeCartAsync(string cartToken, string userId);
    }
}
