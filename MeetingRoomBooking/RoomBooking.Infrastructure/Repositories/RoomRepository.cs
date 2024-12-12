using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.DTO;
using RoomBooking.Domain.Entities;
using RoomBooking.Domain.Repositories;
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

        public async Task<IList<Room>> GetAllRoomAsync()
        {
            return await GetAllAsync();
        }

        public async Task CreateRoomAsync(Room room)
        {
            await AddAsync(room);
        }
    }
}
