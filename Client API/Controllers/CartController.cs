using Core.DTOs;
using Core.Interfaces;
using Core.Models.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cart)
        {
            cartService = cart;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddtoCartAsync(CartOwner cartOwner, AddToCartDTO addToCart)
        {
            if (cartOwner is null || addToCart is null)
            {
                return BadRequest();
            }
            await cartService.AddToCartAsync(cartOwner, addToCart);
            return Ok();
        }

        [HttpPut("update-cart")]
        public async Task<IActionResult> UpdateCartItemQuantityAsync(CartOwner cartOwner, UpdateCartDTO updateCart)
        {
            if (cartOwner is null || updateCart is null)
            {
                return BadRequest();
            }
            await cartService.UpdateCartItemQuantityAsync(cartOwner, updateCart);
            return Ok();
        }

        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCartAsync(CartOwner cartOwner)
        {
            if (cartOwner is null)
            {
                return BadRequest();
            }
            await cartService.GetCartAsync(cartOwner);
            return Ok();
        }

        [HttpPut("remove-from-cart")]
        public async Task<IActionResult> RemoveFromCartAsync(CartOwner cartOwner, int cartItemId)
        {
            if (cartOwner is null || cartItemId ==0)
            {
                return BadRequest();
            }
            await cartService.RemoveFromCartAsync(cartOwner, cartItemId);
            return Ok();
        }

        [HttpDelete("clear-cart")]
        public async Task<IActionResult> ClearCartAsync(CartOwner cartOwner)
        {
            if (cartOwner is null)
            {
                return BadRequest();
            }
            await cartService.ClearCartAsync(cartOwner);
            return Ok();
        }

        [HttpPost("merge-cart")]
        public async Task<IActionResult> MergeCartAsync(string cartToken, string userId)
        {
            if (string.IsNullOrEmpty(cartToken) || string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            await cartService.MergeCartAsync(cartToken, userId);
            return Ok();
        }
    }
}
