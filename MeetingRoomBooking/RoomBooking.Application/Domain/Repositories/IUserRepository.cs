using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IUserRepository
    {
        Task CreateUploadedUserAsync(UploadedUser user);
        Task<IList<UploadedUser>> CheckPreviousLogging(string userEmail, bool isTrackingOff);
    }
}
