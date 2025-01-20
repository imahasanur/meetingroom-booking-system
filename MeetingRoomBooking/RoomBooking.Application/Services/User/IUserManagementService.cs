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
        Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail);
        Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password);
        Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(CreateRegisterUserDTO userDTO);
        Task CreateUploadedUserAsync(DTO.CreateUserDTO user);
        Task<bool> CheckPreviousLogging(string userEmail, bool isTrackingOff);
        Task<bool> UpdateLoggedInState(string userEmail);
        Task LogoutAsync();
    }
}
