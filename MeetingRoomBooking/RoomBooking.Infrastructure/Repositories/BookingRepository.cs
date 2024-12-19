using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RoomBooking.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Event, Guid>, IBookingRepository
    {

        public BookingRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        public async Task EditBookingAsync(Event eventEntity)
        {
            await EditAsync(eventEntity);
        }

        public async Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end)
        {
            Expression<Func<Event, bool>> expression = x => start.Date <= x.Start.Date && end.Date >= x.End.Date;

            return await GetAsync(expression, null);
        }

        public async Task CreateBookingAsync(Event eventEntity)
        {
            await AddAsync(eventEntity);
        }



        //public async Task DeleteRoomAsync(Room room)
        //{
        //    await RemoveAsync(room);
        //}

        //public async Task<IList<Room>> CheckRoomRedundancy(Guid id, string location, string name)
        //{
        //    Expression<Func<Room, bool>> expression = x => x.Id != id && x.Location == location && x.Name == name;

        //    return await GetAsync(expression, null);
        //}

        //public async Task<IList<Room>> CheckRoomRedundancy(string location, string name)
        //{
        //    Expression<Func<Room, bool>> expression = x => x.Location == location && x.Name == name;

        //    return await GetAsync(expression, null);
        //}
    }
}
