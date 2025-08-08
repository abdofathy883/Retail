using Core.DTOs.AuthDTOs;
using Core.Interfaces;
using Core.Interfaces.BackStore_Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Client_API.Controllers.BackStoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminController : ControllerBase
    {
        private readonly IAuthAdminService authAdminService;
        public AuthAdminController(IAuthAdminService service)
        {
            authAdminService = service;
        }
        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("add-new-user")]
        public async Task<IActionResult> AddNewUserAsync(POSRegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await authAdminService.AddNewUserAsync(register);
            if (!result.IsAuthenticated)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { result.Message });
        }
        
        //[Authorize(Roles = "Admin,SuperAdmin,Manager")]
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLoginAsync(LoginDTO login)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            if (!ModelState.IsValid)
            {
                stopwatch.Stop();
                return BadRequest(stopwatch.ElapsedMilliseconds);
            }
            try
            {
                var result = await authAdminService.LoginAsync(login);
                if (!result.IsAuthenticated)
                {
                    stopwatch.Stop();
                    return BadRequest(stopwatch);
                    //return BadRequest(new { result.Message });
                }
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //return Ok(stopwatch);
                return Ok(result);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return StatusCode(500, $"{ex.Message} - {stopwatch.ElapsedMilliseconds} ms");
                //return StatusCode(500, $"{ex}");
            }
        }

        [Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDTO register)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await authAdminService.UpdateUserAsync(userId, register);
            if (!result.IsAuthenticated)
            {
                return BadRequest(new { result.Message });
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("permanent-delete-user")]
        public async Task<IActionResult> PermanentDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            await authAdminService.DeleteUserAsync(userId);
            return Ok();
        }
        [HttpPut("delete-user")]
        public async Task<IActionResult> SoftDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            var result = await authAdminService.SoftDeleteUserAsync(userId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            var result = await authAdminService.GetUserByIdAsync(userId);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await authAdminService.GetAllUsersAsync();
            if (result is null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
