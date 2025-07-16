﻿using Core.Interfaces;
using Core.Models;
using Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class JWTService: IJWTService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JWTSettings jWTSettings;

        public JWTService(UserManager<ApplicationUser> user, JWTSettings settings)
        {
            userManager = user;
            jWTSettings = settings;
        }
        public async Task<string> GenerateAccessTokenAsync(ApplicationUser appUser)
        {
            var userClaims = await userManager.GetClaimsAsync(appUser);
            var userRoles = await userManager.GetRolesAsync(appUser);
            var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, appUser.UserName??""),
            new (JwtRegisteredClaimNames.Email, appUser.Email ?? ""),
            new ("uid", appUser.Id)
        }.Union(userClaims)
             .Union(roleClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTSettings.Key));

            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jWTSettings.Issuer,
                audience: jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jWTSettings.ExpirationMinutes + 60),
                signingCredentials: signingCredentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return accessToken;
        }

        public Task<RefreshToken> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreateOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(jWTSettings.RefreshTokenExpirationDays)
            });
        }
    }
}
