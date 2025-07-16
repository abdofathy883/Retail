using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepo<ApplicationUser> authService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJWTService jWTService;
        private readonly EmailService emailService;
        public AuthService(IGenericRepo<ApplicationUser> repo, 
            UserManager<ApplicationUser> user,
            IJWTService jWTService,
            EmailService emailService)
        {
            authService = repo;
            userManager = user;
            this.jWTService = jWTService;
            this.emailService = emailService;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception();
            }
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }
            else
                return true;
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception();
            }

            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ConcurrencyStamp = user.ConcurrencyStamp
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO login)
        {
            var authDTO = new AuthResponseDTO();
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user is null || await userManager.CheckPasswordAsync(user, login.Password))
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "Invalid";
                return authDTO;
            }

            if (user.IsDeleted)
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "Invalid";
                return authDTO;
            }

            var roles = await userManager.GetRolesAsync(user);
            authDTO.UserId = user.Id;
            authDTO.FirstName = user.FirstName;
            authDTO.LastName = user.LastName;
            authDTO.Email = user.Email;
            authDTO.UserName = user.UserName;
            authDTO.PhoneNumber = user.PhoneNumber ?? string.Empty;
            authDTO.Roles = roles.ToList();
            authDTO.IsAuthenticated = true;
            authDTO.Token = await jWTService.GenerateAccessTokenAsync(user);
            authDTO.ConcurrencyStamp = user.ConcurrencyStamp;

            if (user.RefreshTokens.Any(u => u.IsActive))
            {
                var ActiveRefreshToken = user.RefreshTokens.First(t => t.IsActive);
                authDTO.RefreshToken = ActiveRefreshToken.Token;
                authDTO.RefreshTokenExpiration = ActiveRefreshToken.ExpiresOn;
            }
            else
            {
                var RefreshToken = await jWTService.GenerateRefreshTokenAsync();
                authDTO.RefreshToken = RefreshToken.Token;
                authDTO.RefreshTokenExpiration = RefreshToken.ExpiresOn;
                user.RefreshTokens.Add(RefreshToken);
                await userManager.UpdateAsync(user);
            }

            authDTO.Message = "تم تسجيل الدخول بنجاح";
            Console.WriteLine($"ConcurrencyStamp: {user.ConcurrencyStamp}");
            return authDTO;
        }

        public Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDTO> RegisterCustomerAsync(RegisterDTO newUser)
        {
            var validateErrors = await ValidateRegisterAsync(newUser);
            if (validateErrors is not null && validateErrors.Count > 0)
            {
                return FailResult(string.Join(", ", validateErrors));
            }

            var user = new ApplicationUser
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.Email.Split("@")[0],
                Email = newUser.Email,
                EmailConfirmed = true,
                PhoneNumber = newUser.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                return FailResult(string.Join(", ", validateErrors));
            }

            await userManager.AddToRoleAsync(user, UserRoles.customer.ToString());

            var authDTO = new AuthResponseDTO
            {
                IsAuthenticated = true,
                Message = "تم استلام طلبكم وسيتم التواصل معكم قريبا"
            };
            return authDTO;
        }

        public async Task<bool> SoftDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception();
            }
            user.IsDeleted = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }
            else
            {
                return true;
            }
        }

        public async Task<AuthResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updatedUser)
        {
            if (string.IsNullOrEmpty(userId) || updatedUser is null)
            {
                throw new Exception();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception();
            }
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.ConcurrencyStamp = updatedUser.ConcurrencyStamp;
            await userManager.UpdateAsync(user);
            await userManager.ChangePasswordAsync(user, user.PasswordHash, updatedUser.Password);

            var authDTO = new AuthResponseDTO
            {
                IsAuthenticated = true,
                Message = "done"
            };
            return authDTO;
        }

        public static AuthResponseDTO FailResult(string message)
        {
            return new AuthResponseDTO
            {
                IsAuthenticated = false,
                Message = message
            };
        }

        public async Task<List<string>> ValidateRegisterAsync(RegisterDTO registerDTO)
        {
            var errors = new List<string>();

            // Email Validation
            if (string.IsNullOrWhiteSpace(registerDTO.Email))
            {
                errors.Add("بريد الكتروني غير صالح");
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                errors.Add("هذا الايميل موجود بالفعل");
            }

            //Password Validation
            if (string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                errors.Add("الرقم السري مطلوب");
            }
            else if (registerDTO.Password.Length < 6)
            {
                errors.Add("الرقم السري يجب ان يكون 6 احرف على الاقل");
            }

            //Phone 
            if (string.IsNullOrWhiteSpace(registerDTO.PhoneNumber))
            {
                errors.Add("رقم الهاتف مطلوب");
            }

            //Name
            if (string.IsNullOrWhiteSpace(registerDTO.FirstName))
            {
                errors.Add("الاسم الاول مطلوب");
            }
            if (string.IsNullOrWhiteSpace(registerDTO.LastName))
            {
                errors.Add("الاسم الاخير مطلوب");
            }

            return errors;
        }
    }
}
