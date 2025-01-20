using RoomBooking.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Domain.Repositories
{
    public interface IGuestRepository: IRepositoryBase<Guest, Guid>
    {
        Task AddGuestAsync(Guest guest);
        Task RemoveGuestAsync(Guest guest);
    }
}
