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
    public class EventTimeRepository : Repository<EventTime, Guid>, IEventTimeRepository
    {

        public EventTimeRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

    }

}
