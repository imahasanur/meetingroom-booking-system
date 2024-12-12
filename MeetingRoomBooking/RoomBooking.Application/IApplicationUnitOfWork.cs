using RoomBooking.Application.Services.Room;
using RoomBooking.Domain;
using RoomBooking.Domain.Repositories;
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
    }
}
