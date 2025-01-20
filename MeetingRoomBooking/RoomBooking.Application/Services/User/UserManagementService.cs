using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.User
{
    public class UserManagementService: IUserManagementService
    {

        private readonly IApplicationUnitOfWork _unitOfWork;

        public UserManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(CreateRegisterUserDTO userDTO)
        {
            RoomBooking.Domain.DTO.CreateRegisterUserDTO user = new RoomBooking.Domain.DTO.CreateRegisterUserDTO()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                FullName = userDTO.FullName,
                Department = userDTO.Department,
                MemberPin = userDTO.MemberPin,
                Phone = userDTO.Phone,
                Password = userDTO.Password,
                CreatedAtUtc = userDTO.CreatedAtUtc,
            };
            var response = await _unitOfWork.IdentityUserRepository.RegisterAsync(user);

            return response;
        }

        public async Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail)
        {
            var userDTO = await _unitOfWork.IdentityUserRepository.GetUserByMailAsync(userEmail);

            RoomBooking.Application.DTO.GetRegisterUserDTO registerUserDTO = new RoomBooking.Application.DTO.GetRegisterUserDTO()
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
                LockoutEnd = userDTO.LockoutEnd,
                LastUpdatedAtUTC = userDTO.LastUpdatedAtUTC,
            };

            return registerUserDTO;
        }

        public async Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password)
        {
            RoomBooking.Domain.DTO.GetRegisterUserDTO registerDTO = new RoomBooking.Domain.DTO.GetRegisterUserDTO()
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
                LockoutEnd = userDTO.LockoutEnd,
                LastUpdatedAtUTC = userDTO.LastUpdatedAtUTC,
            };

            var response = await _unitOfWork.IdentityUserRepository.CheckPasswordAsync(registerDTO, password);

            return response;
        }

        public async Task CreateUploadedUserAsync(CreateUserDTO user)
        {
            var uploadedUser = new UploadedUser
            {
                UserEmail = user.UserEmail,
                IsLoggedIn = user.IsLoggedIn,
                CreatedAtUTC = user.CreatedAtUTC,
            };

            await _unitOfWork.UserRepository.CreateUploadedUserAsync(uploadedUser);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> CheckPreviousLogging(string userEmail, bool isTrackingOff)
        {
            bool isPreviousLoggedIn = false;

            var user = await _unitOfWork.UserRepository.CheckPreviousLogging(userEmail,isTrackingOff);

            if (user == null || user.Count == 0)
            {
                isPreviousLoggedIn = true;
                
                return isPreviousLoggedIn;
            }

            var loggedInUsr = user[0];

            if(loggedInUsr.IsLoggedIn == false)
            {
               isPreviousLoggedIn = false;
            }
            else
            {
                isPreviousLoggedIn = true;
            }

            return isPreviousLoggedIn;
        }

        public async Task<bool> UpdateLoggedInState(string userEmail)
        {
            bool isPreviousLoggedIn = true;

            var user = await _unitOfWork.UserRepository.CheckPreviousLogging(userEmail, false);
            var loggedInUsr = user[0];

            loggedInUsr.IsLoggedIn = true;
            loggedInUsr.LastUpdatedAtUTC = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return isPreviousLoggedIn;
        }

        public async Task LogoutAsync()
        {
            await _unitOfWork.IdentityUserRepository.LogoutAsync();
        }

     
    }
}
