using Microsoft.EntityFrameworkCore;
using RoomBooking.Application;
using RoomBooking.Application.Domain.Repositories;
using RoomBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IRoomRepository RoomRepository { get; set; }
        private readonly IApplicationDbContext _context;
        public ApplicationUnitOfWork(IApplicationDbContext dbContext, IRoomRepository roomRepository) : base((DbContext)dbContext)
        {
            _context = (ApplicationDbContext)dbContext;
            RoomRepository = roomRepository;
        }
    }
}
