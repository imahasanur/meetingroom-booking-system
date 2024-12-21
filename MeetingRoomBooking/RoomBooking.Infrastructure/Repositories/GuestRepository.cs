using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Repositories
{
    public class GuestRepository : Repository<Guest, Guid>, IGuestRepository
    {

        public GuestRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task AddGuestAsync(Guest guest)
        {
            await AddAsync(guest);
        }

        public async Task RemoveGuestAsync(Guest guest)
        {
            await RemoveAsync(guest);
        }
    }
}
