using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.Booking
{
    public interface IBookingManagementService
    {
        //Task<GetRoomDTO> GetRoomAsync(Guid id);
        //Task<IList<GetRoomDTO>> GetAllRoomAsync();
        Task<string> CreateBookingAsync(CreateEventDTO eventDTO);
        Task<IList<GetEventDTO>> GetAllEventAsync(DateTime start, DateTime end, string? user);

        Task<string> EditBookingAsync(EditEventDTO eventDTO);
        //Task DeleteRoomAsync(RoomDTO getRoomDTO);
    }
}
