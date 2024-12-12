using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.Room
{
    public interface IRoomManagementService
    {
        Task<IList<GetRoomDTO>> GetAllRoomAsync();
        Task CreateRoomAsync(CreateRoomDTO roomDTO);
    }
}
