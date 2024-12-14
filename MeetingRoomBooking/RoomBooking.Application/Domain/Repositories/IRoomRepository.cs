using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IRoomRepository:IRepositoryBase<Room, Guid>
    {
        Task<Room> GetRoomAsync(Guid id);
        public Task<IList<Room>> GetAllRoomAsync();
        Task CreateRoomAsync(CreateRoomDTO roomDTO);
        Task DeleteRoomAsync(RoomDTO roomDTO);
    }
}