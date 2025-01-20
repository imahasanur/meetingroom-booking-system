using RoomBooking.Domain.Domain.Entities;

namespace RoomBooking.Domain.Domain.Repositories
{
    public interface IRoomRepository:IRepositoryBase<Room, Guid>
    {
        Task<IList<Room>?> CheckRoomRedundancy(Guid id, string location, string name);
        Task<IList<Room>> CheckRoomRedundancy(string location, string name);
        Task<Room> GetRoomAsync(Guid id, bool isTracking);
        public Task<IList<Room>> LoadRoomAsync();
        Task CreateRoomAsync(Room room);
        Task EditRoomAsync(Room room);
        Task DeleteRoomAsync(Room room);
    }
}