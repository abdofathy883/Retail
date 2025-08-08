using Core.DTOs.AuthDTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.BackStore_Interfaces;
using Core.Models;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.BackStore_Services
{
    public class AuthAdminService : IAuthAdminService
    {
        private readonly IGenericRepo<ApplicationUser> authRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJWTService jWTService;
        public AuthAdminService(
            IGenericRepo<ApplicationUser> _authRepo,
            UserManager<ApplicationUser> _userManager,
            IJWTService jWTService
            )
        {
            authRepo = _authRepo;
            userManager = _userManager;
            this.jWTService = jWTService;
        }


        public async Task<AuthResponseDTO> AddNewUserAsync(POSRegisterDTO newUser)
        {
            var validateErrors = await ValidateRegisterAsync(newUser);
            if (validateErrors is not null && validateErrors.Count > 0)
                return FailResult(string.Join(", ", validateErrors));

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
                return FailResult(string.Join(", ", validateErrors ?? new List<string>()));

            if (newUser.Role == UserRoles.SuperAdmin.ToString())
            {
                await userManager.AddToRoleAsync(user, UserRoles.SuperAdmin.ToString());
            } 
            else if (newUser.Role == UserRoles.Admin.ToString())
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            }
            else if (newUser.Role == UserRoles.Cashier.ToString())
            {
                await userManager.AddToRoleAsync(user, UserRoles.Cashier.ToString());
            }
            else if (newUser.Role == UserRoles.Manager.ToString())
            {
                await userManager.AddToRoleAsync(user, UserRoles.Manager.ToString());
            }
            else
            {
                return FailResult("الدور غير صالح");
            }

            var authDTO = new AuthResponseDTO
            {
                IsAuthenticated = true,
                Message = "تم تسجيل حساب جديد بنجاح"
            };
            return authDTO;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO login)
        {
            var authDTO = new AuthResponseDTO();
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, login.Password))
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "لا يوجد حساب بهذه البيانات";
                return authDTO;
            }

            if (user.IsDeleted)
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "هذا الحساب غير موجود";
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

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await GetUserOrThrow(userId);

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception();
            else
                return true;
        }

        private async Task<ApplicationUser> GetUserOrThrow(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user ?? throw new InValidObjectException("User not found");
        }

        public static AuthResponseDTO FailResult(string message)
        {
            return new AuthResponseDTO
            {
                IsAuthenticated = false,
                Message = message
            };
        }

        public async Task<List<string>> ValidateRegisterAsync(POSRegisterDTO registerDTO)
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

        public async Task<AuthResponseDTO> GetUserByIdAsync(string userId)
        {
            return await GetUserOrThrow(userId)
                .ContinueWith(task => new AuthResponseDTO
                {
                    FirstName = task.Result.FirstName,
                    LastName = task.Result.LastName,
                    Email = task.Result.Email,
                    PhoneNumber = task.Result.PhoneNumber,
                    IsDeleted = task.Result.IsDeleted,
                    ConcurrencyStamp = task.Result.ConcurrencyStamp
                });
        }

        public async Task<List<AuthResponseDTO>> GetAllUsersAsync()
        {
            var users = await authRepo.GetAllForAdminAsync()
                ?? throw new InValidObjectException("لا يوجد مستخدمين");

            return users.Select(u => new AuthResponseDTO
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsDeleted = u.IsDeleted,
                ConcurrencyStamp = u.ConcurrencyStamp,
                Roles = userManager.GetRolesAsync(u).Result.ToList()
            }).ToList();
        }

        public async Task<AuthResponseDTO> UpdateUserAsync(string userId, UpdateUserDTO updatedUser)
        {
            if (updatedUser is null)
                throw new InValidObjectException("Updated user data cannot be null");

            var user = await GetUserOrThrow(userId);

            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;

            if (!string.IsNullOrWhiteSpace(updatedUser.Email))
            {
                await userManager.ChangeEmailAsync(user, updatedUser.Email, userManager.GenerateChangeEmailTokenAsync(user, updatedUser.Email).Result);
            }
            user.PhoneNumber = updatedUser.PhoneNumber ?? user.PhoneNumber;

            user.ConcurrencyStamp = updatedUser.ConcurrencyStamp ?? user.ConcurrencyStamp;
            var result = await userManager.UpdateAsync(user);   
            if (!result.Succeeded)
                throw new Exception("فشل في تحديث المستخدم");
            return new AuthResponseDTO
            {
                Message = "تم تحديث بيانات المستخدم بنجاح",
                IsAuthenticated = true
            };
        }

        public async Task<bool> SoftDeleteUserAsync(string userId)
        {
            var user = await GetUserOrThrow(userId);
            if (user.IsDeleted)
                throw new InValidObjectException("هذا الحساب غير موجود");

            user.IsDeleted = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("فشل في حذف المستخدم");

            return true;
        }

        public Task ResetPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}
