using Core.DTOs.VariantsDTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService colorService;
        public ColorController(IColorService _colorService)
        {
            colorService = _colorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllColorsAsync()
        {
            try
            {
                var colors = await colorService.GetColorsAsync();
                return Ok(colors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetColorByIdAsync(int id)
        {
            try
            {
                var color = await colorService.GetColorByIdAsync(id);
                return Ok(color);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddNewColorAsync(AddColorDTO color)
        {
            try
            {
                var newColor = await colorService.AddNewColorAsync(color);
                return Ok(newColor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> UpdateColorAsync(UpdateColorDTO updateColor)
        {
            try
            {
                var updatedColor = await colorService.UpdateColorAsync(updateColor);
                return Ok(updatedColor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> VisibilityToggleColorAsync(int id)
        {
            try
            {
                var deletedColor = await colorService.VisibilityToggleAsync(id);
                return Ok(deletedColor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColorAsync(int id)
        {
            try
            {
                var result = await colorService.DeleteColorAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
