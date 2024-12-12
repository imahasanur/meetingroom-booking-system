using RoomBooking.Domain.Entities;

namespace RoomBooking.Domain.Repositories
{
    public interface IRoomRepository:IRepositoryBase<Room, Guid>
    {
        public Task<IList<Room>> GetAllRoomAsync();
        Task CreateRoomAsync(Room room);
    }
}