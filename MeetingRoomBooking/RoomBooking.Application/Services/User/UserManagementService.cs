using RoomBooking.Application.Domain.Entities;
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
            var response = await _unitOfWork.IdentityUserRepository.RegisterAsync(userDTO);

            return response;
        }

        public async Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail)
        {
            var user = await _unitOfWork.IdentityUserRepository.GetUserByMailAsync(userEmail);
            return user;
        }

        public async Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password)
        {
            var response = await _unitOfWork.IdentityUserRepository.CheckPasswordAsync(userDTO, password);

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
