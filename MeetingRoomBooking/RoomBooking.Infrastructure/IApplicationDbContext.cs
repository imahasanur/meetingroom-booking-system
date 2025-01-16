using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure
{
    public interface IApplicationDbContext
    {
        public DbSet<Room> Room { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<EventTime> Time { get; set; }
        public DbSet<UploadedUser> UploadedUser { get; set; }
        public DbSet<ApplicationUser> IdentityUser { get; set; }
    }
}
