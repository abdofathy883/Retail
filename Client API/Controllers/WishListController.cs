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
        public async Task<IActionResult> AddToWishListAsync(string customerId, Guid productId)
        {
            if (string.IsNullOrEmpty(customerId) || productId == Guid.Empty)
            {
                return BadRequest();
            }
            await wishListService.AddToWishlistAsync(customerId, productId);
            return Ok();
        }

        [HttpPut("clear-wishlist")]
        public async Task<IActionResult> ClearWishListAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest();
            }
            await wishListService.ClearWishlistAsync(customerId);
            return Ok();
        }
        [HttpPut("modify-wishlist")]
        public async Task<IActionResult> DeleteFromWishListAsync(string customerId, int wishlistItemId)
        {
            if (string.IsNullOrEmpty(customerId) || wishlistItemId == 0)
            {
                return BadRequest();
            }
            await wishListService.RemoveFromWishlistAsync(customerId, wishlistItemId);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetWishListAsync(string customerId)
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
