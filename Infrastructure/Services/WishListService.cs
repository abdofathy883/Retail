using AutoMapper;
using Core.DTOs;
using Core.DTOs.WishListDTOs;
using Core.Interfaces;
using Core.Models;
using Core.Models.ValueObjects;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class WishListService: IWishListService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepo<WishList> wishlistRepo;
        private readonly IGenericRepo<WishListItem> wishlistItemRepo;
        private readonly IGenericRepo<Product> productRepo;
        private readonly IGenericRepo<ProductVariant> productVarientRepo;
        private readonly IMapper mapper;

        public WishListService(UserManager<ApplicationUser> _userManager,
            IGenericRepo<WishList> _wishlistRepo,
            IGenericRepo<WishListItem> _wishlistItemRepo,
            IGenericRepo<Product> productRepo,
            IGenericRepo<ProductVariant> productVarientRepo,
            IMapper _mapper)
        {
            userManager = _userManager;
            wishlistRepo = _wishlistRepo;
            wishlistItemRepo = _wishlistItemRepo;
            this.productRepo = productRepo;
            this.productVarientRepo = productVarientRepo;
            mapper = _mapper;
        }

        public async Task<WishListDTO> AddToWishlistAsync(AddToWishListDTO wishListDTO)
        {
            await GetUserOrThrow(wishListDTO.CustomerId);

            var wishlist = await GetWishListForUserAsync(wishListDTO.CustomerId) 
                ?? throw new InValidObjectException("Wishlist not found");
            
            var varient = (await productVarientRepo.GetByIdAsync(wishListDTO.productVarientId))
                ?? throw new InValidPropertyIdException("Product variant not found");

            var isAlreadyInWishlist = await IsInWishlistAsync(wishListDTO.CustomerId, wishListDTO.productVarientId);

            if (isAlreadyInWishlist)
                throw new InValidObjectException("Product already in wishlist");

            if (wishlist.WishListItems == null)
                wishlist.WishListItems = new List<WishListItem>();

            wishlist.WishListItems.Add(new WishListItem
            {
                ProductVarientId = wishListDTO.productVarientId,
                WishListId = wishlist.Id,
            });
            wishlist.LastUpdatedAt = DateTime.UtcNow;

            await wishlistItemRepo.SaveAllAsync();

            varient.NuOfPutInWishList++;
            await productVarientRepo.SaveAllAsync();

            return mapper.Map<WishListDTO>(wishlist);
        }

        public async Task<WishListDTO> ClearWishlistAsync(string customerId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await GetWishListForUserAsync(customerId);

            if (wishlist is null)
                throw new InValidObjectException("");

            wishlist.WishListItems.Clear();
            wishlistRepo.Update(wishlist);
            await wishlistRepo.SaveAllAsync();

            return mapper.Map<WishListDTO>(wishlist);
        }

        public async Task<WishListDTO> RemoveFromWishlistAsync(string customerId, int wishlistItemId)
        {
            await GetUserOrThrow(customerId);

            var wishlistItem = await wishlistItemRepo.GetByIdAsync(wishlistItemId);
            if (wishlistItem is null)
                throw new InValidObjectException("Wishlist item not found");

            var wishlist = await GetWishListForUserAsync(customerId);
            if (wishlistItem is null)
                throw new InValidObjectException("");

            wishlist.WishListItems.Remove(wishlistItem);
            wishlistRepo.Update(wishlist);
            await wishlistRepo.SaveAllAsync();

            return mapper.Map<WishListDTO>(wishlist);

        }

        public async Task<WishListDTO> GetWishlistByUserIdAsync(string customerId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await GetWishListForUserAsync(customerId);

            if (wishlist is null)
                throw new InValidObjectException("Wishlist not found");

            return mapper.Map<WishListDTO>(wishlist);

        }

        private async Task<ApplicationUser> GetUserOrThrow(string customerId)
        {
            var user = await userManager.FindByIdAsync(customerId);
            return user ?? throw new Exception("User not found");
        }
        public async Task<bool> IsInWishlistAsync(string customerId, int productVarientId)
        {
            await GetUserOrThrow(customerId);

            var wishlist = await GetWishListForUserAsync(customerId);

            var product = await productRepo.GetByIdAsync(productVarientId);

            if (wishlist is null || product is null)
                throw new InValidObjectException("Wishlist or product not found");

            return wishlist.WishListItems.Any(i => i.ProductVarientId == productVarientId);
        }

        private async Task<WishList?> GetWishListForUserAsync(string customerId)
        {
            if (!string.IsNullOrEmpty(customerId))
            {
                var wishlist = await wishlistRepo.FindAsync(c => c.UserId == customerId && !c.IsDeleted)
                    ?? throw new InValidObjectException("");
                return (WishList?)wishlist;
            }
            else
                throw new InValidObjectException("");
        }

    }
}
