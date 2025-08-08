using Core.DTOs.AuthDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;
        public AuthController(IAuthService auth)
        {
            authService = auth;
        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await authService.LoginAsync(login);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(new { result.Message });
                }
                if (result.RefreshToken is not null)
                {
                    SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiration);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await authService.RegisterCustomerAsync(register);
            if (!result.IsAuthenticated)
            {
                return BadRequest(new { result.Message });
            }
            
            return Ok(new { result.Message });
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            var user = await authService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPut("delete-user")]
        public async Task<IActionResult> SoftDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            await authService.SoftDeleteUserAsync(userId);
            return Ok();

        }
        
    }
}
