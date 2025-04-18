﻿using RoomBooking.Application.Services.Room;
using RoomBooking.Domain.Domain;
using RoomBooking.Domain.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IRoomRepository RoomRepository { get; set; }
        public IBookingRepository BookingRepository { get; set; }
        public IGuestRepository GuestRepository { get; set; }
        public IEventTimeRepository EventTimeRepository { get; set; }
        public IUserRepository UserRepository { get; set;}
        public IIdentityUserRepository IdentityUserRepository { get; set; }
    }
}
