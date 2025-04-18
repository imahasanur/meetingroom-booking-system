﻿using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Domain.Repositories
{
    public interface IIdentityUserRepository
    {
        Task<GetRegisterUserDTO> GetUserByMailAsync(string userEmail);
        Task<bool> CheckPasswordAsync(GetRegisterUserDTO userDTO, string password);
        Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(CreateRegisterUserDTO userDTO);
        Task LogoutAsync();
    }
}
