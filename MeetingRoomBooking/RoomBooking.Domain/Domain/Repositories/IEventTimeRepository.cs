using RoomBooking.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Domain.Repositories
{
    public interface IEventTimeRepository: IRepositoryBase<EventTime, Guid>
    {
        Task<IList<EventTime>> GetTimeLimitAsync();
        Task<EventTime> GetTimeLimitByIdAsync(Guid id);
    }
}
