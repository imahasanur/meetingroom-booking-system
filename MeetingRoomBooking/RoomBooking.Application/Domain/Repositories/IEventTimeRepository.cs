using RoomBooking.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IEventTimeRepository: IRepositoryBase<EventTime, Guid>
    {
    
    }
}
