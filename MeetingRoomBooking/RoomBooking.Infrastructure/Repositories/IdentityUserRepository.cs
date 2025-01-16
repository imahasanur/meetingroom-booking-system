using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.Domain.Repositories;
using RoomBooking.Application.DTO;
using RoomBooking.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Repositories
{
    public class IdentityUserRepository : Repository<ApplicationUser, Guid>, IIdentityUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        public IdentityUserRepository(IApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base((DbContext)context)
        {
            _userManager = userManager;
            _singInManager = signInManager;
        }

        public async Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return null;
            }

            var userDTO = new GetRegisterUserDTO
            {
                Id = user.Id,
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Department = user.Department,
                MemberPin = user.MemberPin,
                CreatedAtUTC = user.CreatedAtUTC,
                LockoutEnd = user.LockoutEnd ?? default(DateTimeOffset),
                LastUpdatedAtUTC = user.LastUpdatedAtUTC,
            };

            return userDTO;
        }

        public async Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password)
        {
            var user = new ApplicationUser
            {
                Id = userDTO.Id,
                AccessFailedCount = userDTO.AccessFailedCount,
                ConcurrencyStamp = userDTO.ConcurrencyStamp,
                EmailConfirmed = userDTO.EmailConfirmed,
                LockoutEnabled = userDTO.LockoutEnabled,
                NormalizedEmail = userDTO.NormalizedEmail,
                NormalizedUserName = userDTO.NormalizedUserName,
                PasswordHash = userDTO.PasswordHash,
                SecurityStamp = userDTO.SecurityStamp,
                TwoFactorEnabled = userDTO.TwoFactorEnabled,
                PhoneNumber = userDTO.PhoneNumber,
                PhoneNumberConfirmed = userDTO.PhoneNumberConfirmed,
                UserName = userDTO.UserName,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Department = userDTO.Department,
                MemberPin = userDTO.MemberPin,
                CreatedAtUTC = userDTO.CreatedAtUTC,
                LockoutEnd = (DateTimeOffset)userDTO.LockoutEnd,
                LastUpdatedAtUTC = userDTO.LastUpdatedAtUTC,
            };

            var response = await _userManager.CheckPasswordAsync(user, password);

            return response;
        }
        public async Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(CreateRegisterUserDTO userDTO)
        {
            var user = new ApplicationUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                FullName = $"{userDTO.FirstName} {userDTO.LastName}",
                Department = userDTO.Department,
                MemberPin = userDTO.MemberPin,
                PhoneNumber = userDTO.Phone,
                CreatedAtUTC = userDTO.CreatedAtUtc
            };
            var response = await _userManager.CreateAsync(user, userDTO.Password);

            if (response.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "user"));

                return (null, "success");
            }
            else
            {
                var errors = response.Errors.Select(e => e.Description).ToList();

                return (errors, "failed");
            }
        }

        public async Task LogoutAsync()
        {
            await _singInManager.SignOutAsync();
        }
    }
}
