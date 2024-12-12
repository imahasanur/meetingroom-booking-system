using Microsoft.EntityFrameworkCore;
using RoomBooking.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
        }
    }
}
