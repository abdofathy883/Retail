using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService wishListService;
        public WishListController(IWishListService service)
        {
            wishListService = service;
        }

        [HttpPost("add-to-wishlist")]
        public async Task<IActionResult> AddToWishListAsync(string customerId, int productVarientId)
        {
            if (string.IsNullOrEmpty(customerId) || productVarientId == 0)
            {
                return BadRequest();
            }
            await wishListService.AddToWishlistAsync(customerId, productVarientId);
            return Ok();
        }

        [HttpDelete("clear-wishlist")]
        public async Task<IActionResult> ClearWishListAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest();
            }
            await wishListService.ClearWishlistAsync(customerId);
            return Ok();
        }
        [HttpPut("remove-from-wishlist")]
        public async Task<IActionResult> RemoveFromWishListAsync(string customerId, int wishlistItemId)
        {
            if (string.IsNullOrEmpty(customerId) || wishlistItemId == 0)
            {
                return BadRequest();
            }
            await wishListService.RemoveFromWishlistAsync(customerId, wishlistItemId);
            return Ok();
        }
        [HttpGet("get-wishlist")]
        public async Task<IActionResult> GetWishListByUserIdAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest();
            }
            await wishListService.GetWishlistByUserIdAsync(customerId);
            return Ok();
        }
    }
}
