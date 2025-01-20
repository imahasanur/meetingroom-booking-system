using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.DTO;
using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Domain.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RoomBooking.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room, Guid>, IRoomRepository
    {

        public RoomRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<Room> GetRoomAsync(Guid id, bool isTracking)
        {
            return await GetByIdAsync(id, isTracking);
        }

        public async Task<IList<Room>> LoadRoomAsync()
        {
            return await GetAsync(null,null,null,true);
        }

        public async Task CreateRoomAsync(Room room)
        {
            await AddAsync(room);
        }

        public async Task EditRoomAsync(Room room)
        {
            await EditAsync(room);
        }

        public async Task DeleteRoomAsync(Room room)
        {
            await RemoveAsync(room);
        }

        public async Task<IList<Room>> CheckRoomRedundancy(Guid id, string location, string name)
        {
            Expression<Func<Room, bool>> expression = x => x.Id != id && x.Location == location && x.Name == name;
            
            return await GetAsync(expression, null);
        }

        public async Task<IList<Room>> CheckRoomRedundancy(string location, string name)
        {
            Expression<Func<Room, bool>> expression = x => x.Location == location && x.Name == name;

            return await GetAsync(expression, null);
        }
    }
}
