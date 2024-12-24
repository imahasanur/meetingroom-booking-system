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

        Task<string> CreateBookingAsync(CreateEventDTO eventDTO, IList<string> allUser, string userClaim);
        Task<IList<GetEventDTO>> GetAllEventAsync(DateTime start, DateTime end, string? user, string? userClaim);

        Task<string> EditBookingAsync(EditEventDTO eventDTO, string currentUser, string userClaim);
        Task<string> EditBookingByIdAsync(EditEventDTO eventDTO, string currentUser, IList<string> allUser, string userClaim);
        Task<string> DeleteBookingAsync(Guid id);
        Task<GetEventDTO> GetEventByIdAsync(Guid id);
    }
}
