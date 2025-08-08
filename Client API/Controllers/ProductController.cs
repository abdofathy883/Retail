using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService service)
        {
            productService = service;
        }
        
        [HttpGet("get-product")]
        public async Task<IActionResult> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest();
            }
            await productService.GetProductByIdAsync(productId);
            return Ok();
        }
        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await productService.GetAllProductsAsync();
            if (products is null)
            {
                return BadRequest();
            }
            return Ok(products);
        }
    }
}
