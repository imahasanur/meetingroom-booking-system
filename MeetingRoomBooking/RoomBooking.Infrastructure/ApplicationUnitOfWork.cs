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
        public IBookingRepository BookingRepository { get; set; }
        public IGuestRepository GuestRepository { get; set; }
        public IEventTimeRepository EventTimeRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        private readonly IApplicationDbContext _context;

        public ApplicationUnitOfWork(IApplicationDbContext dbContext, IRoomRepository roomRepository, IBookingRepository bookingRepository, IGuestRepository guestRepository, IEventTimeRepository eventTimeRepository, IUserRepository userRepository) : base((DbContext)dbContext)
        {
            _context = (ApplicationDbContext)dbContext;
            RoomRepository = roomRepository;
            BookingRepository = bookingRepository;
            GuestRepository = guestRepository;
            EventTimeRepository = eventTimeRepository;
            UserRepository = userRepository;
        }
    }
}
