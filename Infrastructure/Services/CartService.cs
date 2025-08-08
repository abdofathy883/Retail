using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Core.Models.ValueObjects;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepo<Cart> cartRepo;
        private readonly IGenericRepo<CartItem> cartItemRepo;
        private readonly IGenericRepo<Product> productRepo;
        private readonly IGenericRepo<ProductVariant> productVarientRepo;
        private readonly IMapper mapper;

        public CartService(
        UserManager<ApplicationUser> _userManager,
        IGenericRepo<Cart> _cartRepo,
        IGenericRepo<CartItem> _cartItemRepo,
        IGenericRepo<Product> productRepo,
        IGenericRepo<ProductVariant> _productVarientRepo,
        IMapper _mapper)
        {
            userManager = _userManager;
            cartRepo = _cartRepo;
            cartItemRepo = _cartItemRepo;
            this.productRepo = productRepo;
            productVarientRepo = _productVarientRepo;
            mapper = _mapper;
        }
        public async Task<CartDTO> AddToCartAsync(CartOwner cartOwner, AddToCartDTO newCartItem)
        {
            if (cartOwner is null || newCartItem is null)
                throw new InValidObjectException("Cart owner information is missing");

            await EnsureValidCartOwnerAsync(cartOwner);

            var product = await productRepo.GetByIdAsync(newCartItem.ProductId);

            if (product is null)
                throw new InValidObjectException("Product not found");

            var varient = (await productVarientRepo.FindAsync(v => v.Id == newCartItem.ProductVariantId &&
            v.ProductId == product.Id)).FirstOrDefault();

            if (varient is null)
                throw new InValidObjectException("Product variant not found");

            if (varient.Stock < newCartItem.Quantity)
                throw new InValidObjectException("Product is out of stock");

            var cart = await GetCartForOwnerAsync(cartOwner)
                ?? throw new InValidObjectException("Cart not found");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == varient.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += newCartItem.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductVariantId = newCartItem.ProductVariantId,
                    Quantity = newCartItem.Quantity,
                    AddedAt = DateTime.UtcNow,
                    UnitPriceSnapshot = varient.OriginalPrice
                });
            }

            cart.LastUpdatedAt = DateTime.UtcNow;


            if (cart.Id == 0)
                await cartRepo.AddAsync(cart);

            await cartRepo.SaveAllAsync();

            varient.NuOfPutInCart++;
            await productVarientRepo.SaveAllAsync();

            return mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> ClearCartAsync(CartOwner cartOwner)
        {
            if (cartOwner is null)
                throw new InValidObjectException("");

            if (string.IsNullOrEmpty(cartOwner.UserId) && string.IsNullOrEmpty(cartOwner.CartToken))
                throw new InValidPropertyIdException("");
            await EnsureValidCartOwnerAsync(cartOwner);

            var cart = await GetCartForOwnerAsync(cartOwner);

            if (cart is null)
                throw new InValidObjectException("Cart not found");

            cart.Items.Clear();
            cart.LastUpdatedAt = DateTime.UtcNow;

            cartRepo.Update(cart);
            await cartRepo.SaveAllAsync();

            return mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> RemoveFromCartAsync(CartOwner cartOwner, int cartItemId)
        {
            if (cartOwner is null)
                throw new InValidObjectException("");

            if (cartItemId == 0)
                throw new InValidPropertyIdException("");

            await EnsureValidCartOwnerAsync(cartOwner);

            var item = await cartItemRepo.GetByIdAsync(cartItemId);
            if (item is null)
                throw new InValidObjectException("");

            var cart = await GetCartForOwnerAsync(cartOwner);
            if (cart is null)
                throw new InValidObjectException("");

            cart.Items.Remove(item);

            cartRepo.Update(cart);
            await cartRepo.SaveAllAsync();

            return mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> GetCartAsync(CartOwner cartOwner)
        {
            await EnsureValidCartOwnerAsync(cartOwner);

            var cart = await GetCartForOwnerAsync(cartOwner);

            if (cart is null)
            {
                return new CartDTO
                {
                    Id = 0,
                    UserId = cartOwner.UserId,
                    CartToken = null,
                    LastUpdatedAt = DateTime.UtcNow,
                    Items = new List<CartItemDTO>()
                };
            }

            return mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> UpdateCartItemQuantityAsync(CartOwner cartOwner, UpdateCartDTO update)
        {
            if (cartOwner is null || update is null)
                throw new InValidObjectException("");

            var cartItem = await cartItemRepo.GetByIdAsync(update.cartItemId);

            if (cartItem == null || cartItem.IsDeleted)
                throw new InValidObjectException("Cart item not found");

            cartItem.Quantity = update.Quantity;

            cartItemRepo.Update(cartItem);
            await cartItemRepo.SaveAllAsync();

            return new CartDTO
            {
                Id = cartItem.CartId,
                UserId = cartItem.Cart.UserId,
                CartToken = cartItem.Cart.CartToken,
                LastUpdatedAt = cartItem.Cart.LastUpdatedAt,
                Items = cartItem.Cart.Items.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    CartId = i.CartId,
                    ProductVariantId = i.ProductVariantId,
                    Quantity = i.Quantity,
                    UnitPriceSnapshot = i.UnitPriceSnapshot,
                    AddedAt = i.AddedAt
                }).ToList()
            };
        }

        public async Task<CartDTO> MergeCartAsync(string cartToken, string userId)
        {
            if (string.IsNullOrWhiteSpace(cartToken))
                throw new InValidPropertyIdException("Cart token is required");

            if (string.IsNullOrWhiteSpace(userId))
                throw new InValidPropertyIdException("User ID is required");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InValidObjectException("User not found");

            var anonymousCart = (await cartRepo.FindAsync(c => c.CartToken == cartToken && !c.IsDeleted))
                                .FirstOrDefault();

            var userCart = (await cartRepo.FindAsync(c => c.UserId == userId && !c.IsDeleted))
                            .FirstOrDefault();

            if (anonymousCart == null)
                return userCart != null ? mapper.Map<CartDTO>(userCart) : new CartDTO();

            if (userCart == null)
            {
                userCart = new Cart
                {
                    UserId = userId,
                    LastUpdatedAt = DateTime.UtcNow,
                    Items = new List<CartItem>()
                };
                await cartRepo.AddAsync(userCart);
            }

            foreach (var anonItem in anonymousCart.Items)
            {
                var existingItem = userCart.Items.FirstOrDefault(
                    i => i.ProductVariantId == anonItem.ProductVariantId
                );

                if (existingItem != null)
                {
                    existingItem.Quantity += anonItem.Quantity;
                }
                else
                {
                    userCart.Items.Add(new CartItem
                    {
                        ProductVariantId = anonItem.ProductVariantId,
                        Quantity = anonItem.Quantity,
                        UnitPriceSnapshot = anonItem.UnitPriceSnapshot,
                        AddedAt = DateTime.UtcNow
                    });
                }
            }

            userCart.LastUpdatedAt = DateTime.UtcNow;

            // Soft delete the anonymous cart
            anonymousCart.IsDeleted = true;
            anonymousCart.LastUpdatedAt = DateTime.UtcNow;
            cartRepo.Update(anonymousCart);

            cartRepo.Update(userCart);
            await cartRepo.SaveAllAsync();

            return mapper.Map<CartDTO>(userCart);
        }
       
        private async Task EnsureValidCartOwnerAsync(CartOwner owner)
        {
            if (string.IsNullOrEmpty(owner.UserId) && string.IsNullOrEmpty(owner.CartToken))
                throw new ArgumentException("Cart owner must have either a UserId or a CartToken.");

            if (!string.IsNullOrEmpty(owner.UserId))
            {
                var user = await userManager.FindByIdAsync(owner.UserId);
                if (user == null)
                    throw new KeyNotFoundException($"No user found with ID: {owner.UserId}");
            }
        }

        private async Task<Cart?> GetCartForOwnerAsync(CartOwner cartOwner)
        {
            if (!string.IsNullOrEmpty(cartOwner.UserId))
                return (await cartRepo.FindAsync(c => c.UserId == cartOwner.UserId && !c.IsDeleted)).FirstOrDefault();

            if (!string.IsNullOrEmpty(cartOwner.CartToken))
                return (await cartRepo.FindAsync(c => c.CartToken == cartOwner.CartToken && !c.IsDeleted)).FirstOrDefault();

            return null;
        }
    }
}