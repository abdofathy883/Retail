using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService service)
        {
            categoryService = service;
        }

        [HttpPost("create-new-category")]
        public async Task<IActionResult> CreateCategoryAsync([FromForm] Create_UpdateCategoryDTO newCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await categoryService.CreateCategoryAsync(newCategory);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("all-categories")]
        public async Task<IActionResult> GetAllcategoriesAsync()
        {
            try
            {
                var categories = await categoryService.GetAllCategorysAsync();
                if (categories is null)
                {
                    return BadRequest();
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        [HttpGet("category-by-id")]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return BadRequest();
            try
            {
                var category = await categoryService.GetCategoryByIdAsync(categoryId);
                if (category is null)
                    return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("update-category/{oldCategoryId}")]
        public async Task<IActionResult> UpdateCategoryAsync(Guid oldCategoryId, [FromForm] Create_UpdateCategoryDTO newCategory)
        {
            if (oldCategoryId == Guid.Empty || newCategory is null)
            {
                return BadRequest();
            }
            var result = await categoryService.UpdateCategoryAsync(oldCategoryId, newCategory);
            return Ok(result);
        }

        [HttpPut("delete-category/{categoryId}")]
        public async Task<IActionResult> SoftDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest();
            }
            await categoryService.SoftDeleteCategoryAsync(categoryId);
            return Ok();
        }

        [HttpDelete("delete-category-perminent/{categoryId}")]
        public async Task<IActionResult> PerminentDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest();
            }
            await categoryService.DeleteCategoryAsync(categoryId);
            return Ok();
        }
    }
}
