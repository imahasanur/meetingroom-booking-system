using RoomBooking.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IGuestRepository: IRepositoryBase<Guest, Guid>
    {
        Task AddGuestAsync(Guest guest);
        Task RemoveGuestAsync(Guest guest);
    }
}
