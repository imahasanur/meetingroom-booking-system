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
using RoomBooking.Infrastructure.Membership;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomBooking.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Event, Guid>, IBookingRepository
    {

        public BookingRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<IList<Event>> GetBookingByMakerAsync(string createdBy)
        {
            Expression<Func<Event, bool>> expression = x => x.CreatedBy == createdBy && x.End >= DateTime.Now && x.State.Equals("pending");
            return await GetAsync(expression, null, null, true);
        }

        public async Task<IList<Event>> CheckBookingOverlapping(DateTime start, DateTime end, Guid roomId)
        {
            Expression<Func<Event, bool>> expression = x => x.RoomId == roomId && ((start >= x.Start && end >= x.End) || (start <= x.Start && end >= x.End) || (start <= x.Start && end <= x.End) || (start >= x.Start && end <= x.End));
            return await GetAsync(expression, null, null, true);

        }

        public async Task<IList<Event>> CheckAnyRoomBookingOverlappingByUser(DateTime start, DateTime end, string createdBy)
        {
            Expression<Func<Event, bool>> expression = x => x.CreatedBy.Trim().Equals(createdBy.Trim()) && ((start >= x.Start && end >= x.End) || (start <= x.Start && end >= x.End) || (start <= x.Start && end <= x.End) || (start >= x.Start && end <= x.End));
            return await GetAsync(expression, null, null, true);

        }

        
        public async Task<Event> GetEventAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        public async Task EditBookingAsync(Event eventEntity)
        {
            await EditAsync(eventEntity);
        }

        public async Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end, string? user)
        {
            Expression<Func<Event, bool>> expression = null;
            Func<IQueryable<Event>, IIncludableQueryable<Event, object>> include = q => q.Include(e => e.Room).Include(e => e.Guests);

            if (user is not null)
            {
                expression = x => (start.Date <= x.Start.Date && end.Date >= x.End.Date) && (x.Host == user || x.Guests.Any(x => x.User == user) || x.CreatedBy == user);

                return await GetAsync(expression, include);
            }
            else
            {
                expression = x => start.Date <= x.Start.Date && end.Date >= x.End.Date;

                return await GetAsync(expression, null);
            }

        }

        public async Task CreateBookingAsync(Event eventEntity)
        {
            await AddAsync(eventEntity);
        }

        public async Task<Event> GetBookingAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }

        public async Task DeleteBookingAsync(Event eventEntity)
        {
            await RemoveAsync(eventEntity);
        }

        public async Task<IList<Event>> GetBookingByIdAsync(Guid id)
        {
            Expression<Func<Event, bool>> expression = x => x.Id == id;
            Func<IQueryable<Event>, IIncludableQueryable<Event, object>> include = q => q.Include(e => e.Room).Include(e => e.Guests);

            return await GetAsync(expression, include);
        }


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
