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
        Task<GetRoomDTO> GetRoomAsync(Guid id);
        Task<IList<GetRoomDTO>> GetAllRoomAsync();
        Task<string> CreateRoomAsync(CreateRoomDTO roomDTO);
        Task<string> EditRoomAsync(EditRoomDTO roomDTO);
        Task<string> EditRoomSettingAsync(EditRoomSettingDTO roomDTO);
        Task<string> DeleteRoomAsync(Guid id);
    }
}
