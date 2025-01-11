using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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
            
    }
}
