using Microsoft.EntityFrameworkCore;
using RoomBooking.Domain.Entities;
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
    }
}
