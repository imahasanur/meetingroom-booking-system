using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IRoomRepository:IRepositoryBase<Room, Guid>
    {
        Task<IList<Room>?> CheckRoomRedundancy(Guid id, string location, string name);
        Task<Room> GetRoomAsync(Guid id);
        public Task<IList<Room>> GetAllRoomAsync();
        Task CreateRoomAsync(Room room);
        Task EditRoomAsync(Room room);
        Task DeleteRoomAsync(Room room);
    }
}