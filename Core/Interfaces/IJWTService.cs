using Core.Models;

namespace Core.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser appUser);
        Task<RefreshToken> GenerateRefreshTokenAsync();
    }
}
