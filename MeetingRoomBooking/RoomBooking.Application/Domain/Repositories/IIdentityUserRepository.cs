using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IIdentityUserRepository
    {
        Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail);
        Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password);
        Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(CreateRegisterUserDTO userDTO);
        Task LogoutAsync();
    }
}
