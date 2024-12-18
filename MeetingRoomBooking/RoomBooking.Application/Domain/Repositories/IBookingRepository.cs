using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Event, Guid>
    {
        //Task<IList<Room>?> CheckRoomRedundancy(Guid id, string location, string name);
        //Task<IList<Room>> CheckRoomRedundancy(string location, string name);
        //Task<Room> GetRoomAsync(Guid id);
        Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end);
        Task CreateBookingAsync(Event eventEntity);
        //Task EditRoomAsync(Room room);
        //Task DeleteRoomAsync(Room room);
    }
}