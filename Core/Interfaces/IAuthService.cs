using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterCustomerAsync(RegisterDTO newUser);
        Task<AuthResponseDTO> LoginAsync(LoginDTO login);
        Task<AuthResponseDTO> RefreshTokenAsync(string token);
        Task<AuthResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updatedUser);
        Task<bool> SoftDeleteUserAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserDTO> GetUserByIdAsync(string userId);
    }
}
