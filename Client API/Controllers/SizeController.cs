using Core.DTOs.VariantsDTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService sizeService;
        public SizeController(ISizeService _varientService)
        {
            sizeService = _varientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizesAsync()
        {
            try
            {
                var sizes = await sizeService.GetSizesAsync();
                return Ok(sizes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSizeByIdAsync(int id)
        {
            try
            {
                var size = await sizeService.GetSizeByIdAsync(id);
                return Ok(size);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddNewSizeAsync(AddSizeDTO addSizeDTO)
        {
            try
            {
                var newSize = await sizeService.AddNewSizeAsync(addSizeDTO);
                return Ok(newSize);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSizeAsync(int id, string newName)
        {
            try
            {
                var updatedSize = await sizeService.UpdateSizeAsync(id, newName);
                return Ok(updatedSize);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSizeAsync(int id)
        {
            try
            {
                var result = await sizeService.DeleteSizeAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RestoreSizeVisibilityAsync(int id)
        {
            try
            {
                var restoredSize = await sizeService.ToggleVisibilityAsync(id);
                return Ok(restoredSize);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
