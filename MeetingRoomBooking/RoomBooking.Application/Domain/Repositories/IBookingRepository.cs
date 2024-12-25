using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Event, Guid>
    {
        Task<Event> GetEventAsync(Guid id);
        Task<IList<Event>> CheckAnyRoomBookingOverlappingByUser(DateTime start, DateTime end, string createdBy);
        Task<IList<Event>> CheckBookingOverlapping(DateTime start, DateTime end, Guid roomId);
        Task<IList<Event>> CheckEditAnyRoomBookingOverlappingByUser(DateTime start, DateTime end, string createdBy, Guid eventId);
        Task<IList<Event>> CheckEditBookingOverlapping(DateTime start, DateTime end, Guid roomId, Guid eventId);
        Task<IList<Event>> GetBookingByMakerAsync(string createdBy);
        Task<IList<Event>> GetBookingByIdAsync(Guid id);
        Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end, string? user, string? userClaim);
        Task CreateBookingAsync(Event eventEntity);
        Task<Event> GetBookingAsync(Guid id);
        Task EditBookingAsync(Event eventEntity);
        Task DeleteBookingAsync(Event eventEntity);
    }
}