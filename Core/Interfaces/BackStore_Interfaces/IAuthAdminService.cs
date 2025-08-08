using Core.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.BackStore_Interfaces
{
    public interface IAuthAdminService
    {
        Task<AuthResponseDTO> GetUserByIdAsync(string userId);
        Task<List<AuthResponseDTO>> GetAllUsersAsync();
        Task<AuthResponseDTO> AddNewUserAsync(POSRegisterDTO newUser);
        Task<AuthResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updatedUser);
        Task<bool> SoftDeleteUserAsync(string userId);
        Task<AuthResponseDTO> LoginAsync(LoginDTO login);
        Task<bool> DeleteUserAsync(string userId);

        Task ResetPassword(string email);
    }
}
