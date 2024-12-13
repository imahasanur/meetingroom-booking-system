using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IRoomRepository:IRepositoryBase<Room, Guid>
    {
        public Task<IList<Room>> GetAllRoomAsync();
        Task CreateRoomAsync(CreateRoomDTO roomDTO);
    }
}