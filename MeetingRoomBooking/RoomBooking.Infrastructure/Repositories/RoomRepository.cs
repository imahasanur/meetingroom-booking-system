using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room, Guid>, IRoomRepository
    {

        public RoomRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<Room> GetRoomAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IList<Room>> GetAllRoomAsync()
        {
            return await GetAllAsync();
        }

        public async Task CreateRoomAsync(CreateRoomDTO roomDTO)
        {
            var room = new Room()
            {
                Name = roomDTO.Name,
                Details = roomDTO.Details,
                Location = roomDTO.Location,
                Capacity = roomDTO.Capacity,
                CreatedAtUTC = roomDTO.CreatedAtUTC,
                CreatedBy = roomDTO.CreatedBy,
            };
            await AddAsync(room);
        }

        public async Task DeleteRoomAsync(RoomDTO roomDTO)
        {
            var room = new Room()
            {
                Name = roomDTO.Name,
                Details = roomDTO.Details,
                Location = roomDTO.Location,
                Capacity = roomDTO.Capacity,
                CreatedAtUTC = roomDTO.CreatedAtUTC,
                CreatedBy = roomDTO.CreatedBy,
            };
            await RemoveAsync(room);
        }

    }
}
