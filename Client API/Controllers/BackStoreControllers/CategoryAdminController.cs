using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.BackStore_Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client_API.Controllers.BackStoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAdminController : ControllerBase
    {
        private readonly ICategoryAdminService categoryAdminService;
        public CategoryAdminController(ICategoryAdminService service)
        {
            categoryAdminService = service;
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                var result = await categoryAdminService.GetAllCategoriesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var result = await categoryAdminService.GetCategoryByIdAsync(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(Create_UpdateCategoryDTO newCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await categoryAdminService.CreateCategoryAsync(newCategory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{oldCategoryId}")]
        public async Task<IActionResult> UpdateCategoryAsync(Guid oldCategoryId, [FromForm] Create_UpdateCategoryDTO newCategory)
        {
            if (oldCategoryId == Guid.Empty || newCategory is null)
            {
                return BadRequest();
            }
            var result = await categoryAdminService.UpdateCategoryAsync(oldCategoryId, newCategory);
            return Ok(result);
        }

        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> SoftDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest();
            }
            await categoryAdminService.SoftDeleteCategoryAsync(categoryId);
            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> PerminentDeleteCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest();
            }
            await categoryAdminService.DeleteCategoryAsync(categoryId);
            return Ok();
        }

        
    }
}
