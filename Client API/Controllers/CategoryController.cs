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

        [HttpGet]
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
        [HttpGet("{categoryId}")]
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
    }
}
