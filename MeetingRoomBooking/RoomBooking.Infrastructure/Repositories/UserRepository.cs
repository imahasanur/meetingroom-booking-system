using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.Domain.Repositories;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Repositories
{
    public class UserRepository : Repository<UploadedUser, Guid>, IUserRepository
    {
        public UserRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task CreateUploadedUserAsync(UploadedUser user)
        {
            await AddAsync(user);
        }

        public async Task<IList<UploadedUser>> CheckPreviousLogging(string userEmail, bool isTrackingOff)
        {
            Expression<Func<UploadedUser, bool>> filter = x => x.UserEmail.Equals(userEmail);
            return await GetAsync(filter, null, null, isTrackingOff);
        }
    }
}
