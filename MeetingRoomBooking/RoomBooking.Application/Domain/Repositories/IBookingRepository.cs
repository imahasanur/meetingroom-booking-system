using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Event, Guid>
    {
        //Task<IList<Room>?> CheckRoomRedundancy(Guid id, string location, string name);
        //Task<IList<Room>> CheckRoomRedundancy(string location, string name);
        Task<Event> GetEventAsync(Guid id);
        Task<IList<Event>> GetEventByIdAsync(Guid id);
        Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end, string? user);
        Task CreateBookingAsync(Event eventEntity);
        Task<Event> GetBookingAsync(Guid id);
        Task EditBookingAsync(Event eventEntity);
        Task DeleteBookingAsync(Event eventEntity);
    }
}