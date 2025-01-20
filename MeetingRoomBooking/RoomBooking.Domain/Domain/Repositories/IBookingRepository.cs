using RoomBooking.Domain.Domain.Entities;

namespace RoomBooking.Domain.Domain.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Event, Guid>
    {
        Task<Event> GetEventAsync(Guid id);
        Task<IList<Event>> CheckAnyRoomBookingOverlappingByUser(DateTime start, DateTime end, string createdBy);
        Task<IList<Event>> CheckAnyRoomBookingOverlappingByHost(DateTime start, DateTime end, string host);
        Task<IList<Event>> CheckBookingOverlapping(DateTime start, DateTime end, Guid roomId);
        Task<IList<Event>> CheckEditAnyRoomBookingOverlappingByUser(DateTime start, DateTime end, string createdBy, Guid eventId);
        Task<IList<Event>> CheckEditAnyRoomBookingOverlappingByHost(DateTime start, DateTime end, string host, Guid eventId);
        Task<IList<Event>> CheckEditBookingOverlapping(DateTime start, DateTime end, Guid roomId, Guid eventId);
        Task<IList<Event>> GetBookingByMakerAsync(string createdBy);
        Task<IList<Event>> GetBookingByIdAsync(Guid id);
        Task<IList<Event>> LoadEventAsync(DateTime start, DateTime end, string? user, string? userClaim);
        Task<IList<Event>> LoadGuestEventAsync(DateTime start, DateTime end, string? user, string? userClaim);
        Task CreateBookingsAsync(ICollection<Event> eventEntity);
        Task<Event> GetBookingAsync(Guid id);
        Task EditBookingAsync(Event eventEntity);
        Task DeleteBookingAsync(Event eventEntity);
    }
}