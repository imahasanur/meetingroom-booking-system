using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.User
{
    public interface IUserManagementService
    {
        Task CreateUploadedUserAsync(CreateUserDTO user);
    }
}
