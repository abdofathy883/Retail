using Core.DTOs.ProductDTOs;
using Core.DTOs.ProductDTOs.AdminLevel;
using Core.Interfaces;
using Core.Interfaces.BackStore_Interfaces;
using Core.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client_API.Controllers.BackStoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAdminController : ControllerBase
    {
        private readonly IProductAdminService productAdminService;
        private readonly MediaUploadService mediaUploadService;
        public ProductAdminController(
            IProductAdminService service,
            MediaUploadService media
            )
        {
            productAdminService = service;
            mediaUploadService = media;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await productAdminService.GetAllProductsAsync();
                if (products is null) return BadRequest();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDTO createProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await productAdminService.CreateProductAsync(createProduct);
            return Ok(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(Guid oldProductId, UpdateProductDTO newProduct)
        {
            if (!ModelState.IsValid || oldProductId == Guid.Empty)
            {
                return BadRequest();
            }
            await productAdminService.UpdateProductAsync(oldProductId, newProduct);
            return Ok();
        }
        [HttpPut("delete-product")]
        public async Task<IActionResult> SoftDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest();
            }
            await productAdminService.SoftDeleteProductAsync(productId);
            return Ok();
        }

        [HttpDelete("perminint-delete-product")]
        public async Task<IActionResult> PermenintDeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest();
            }
            await productAdminService.DeleteProductAsync(productId);
            return Ok();
        }
    }
}
