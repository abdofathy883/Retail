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
        [HttpPost("add-product")]
        public async Task<IActionResult> CreateProductAsync(CreateProductDTO createProduct) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await productService.CreateProductAsync(createProduct);
            return Ok();
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct)
        {
            if (!ModelState.IsValid || oldProductId == Guid.Empty)
            {
                return BadRequest();
            }
            await productService.UpdateProductAsync(oldProductId, newProduct);
            return Ok();
        }
        [HttpPut("delete-product")]
        public async Task<IActionResult> SoftDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest();
            }
            await productService.SoftDeleteProductAsync(productId);
            return Ok();
        }

        [HttpDelete("perminint-delete-product")]
        public async Task<IActionResult> PermenintDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest();
            }
            await productService.DeleteProductAsync(productId);
            return Ok();
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
