using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class WishListService: IWishListService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepo<WishList> wishlistRepo;
        private readonly IGenericRepo<WishListItem> wishlistItemRepo;
        private readonly IGenericRepo<Product> productRepo;
        public WishListService(UserManager<ApplicationUser> _userManager,
            IGenericRepo<WishList> _wishlistRepo,
            IGenericRepo<WishListItem> _wishlistItemRepo,
            IGenericRepo<Product> productRepo)
        {
            userManager = _userManager;
            wishlistRepo = _wishlistRepo;
            wishlistItemRepo = _wishlistItemRepo;
            this.productRepo = productRepo;
        }

        public async Task<WishListDTO> AddToWishlistAsync(string customerId, Guid productId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await wishlistRepo.FindAsync(w => w.UserId == customerId);
            var wishlistItem = wishlist.FirstOrDefault() ?? new WishList
            {
                UserId = customerId,
                WishListItems = new List<WishListItem>()
            };

            if (wishlistItem is null || wishlistItem.WishListItems is null)
            {
                throw new Exception();
            }

            if (!wishlistItem.WishListItems.Any(i => i.ProductId == productId))
            {
                wishlistItem.WishListItems.Add(new WishListItem
                {
                    ProductId = productId,
                });
                if (wishlistItem.Id == 0)
                {
                    await wishlistRepo.AddAsync(wishlistItem);
                }

                await wishlistRepo.SaveAllAsync();
            }
        }

        public async Task<bool> ClearWishlistAsync(string customerId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await wishlistRepo.FindAsync(w => w.UserId == customerId);
            var wishListItem = wishlist.FirstOrDefault();

            if (wishListItem is null)
            {
                throw new Exception();
            }

            wishListItem.IsDeleted = true;

            return await wishlistRepo.SaveAllAsync();
        }

        public async Task<bool> DeleteFromWishlistAsync(string customerId, int wishlistItemId)
        {
            await GetUserOrThrow(customerId);

            var wishlistItem = await wishlistItemRepo.FindAsync(w => w.WishListId == wishlistItemId);

            if (wishlistItem is null)
            {
                throw new Exception();
            }

            await wishlistItemRepo.SoftDeleteByIdAsync(wishlistItem);
            await wishlistItemRepo.SaveAllAsync();
        }

        public async Task<WishListDTO> GetWishlistByUserIdAsync(string customerId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await wishlistRepo.FindAsync(w => w.UserId == customerId);
            var wishListItem = wishlist.FirstOrDefault();

            if (wishListItem is null || wishListItem.WishListItems is null)
            {
                throw new Exception();
            }

            return new WishListDTO
            {
                Items = wishListItem?.WishListItems.Select(w => new WishListItemDTO
                {

                }).ToList() ?? new List<WishListItemDTO>()
            };
        }

        private async Task<ApplicationUser> GetUserOrThrow(string customerId)
        {
            var user = await userManager.FindByIdAsync(customerId);
            return user ?? throw new Exception("User not found");
        }

        private static WishListDTO MapToCartDTO(WishList wishList)
        {
            return new WishListDTO
            {
                Id = wishList.Id,
                UserId = wishList.UserId,
                LastUpdatedAt = wishList.LastUpdatedAt,
                WishListItems = wishList.WishListItems.Select(i => new WishListItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    WishListId = i.WishListId,
                    AddedAt = i.AddedAt
                }).ToList()
            };
        }
    }
}
