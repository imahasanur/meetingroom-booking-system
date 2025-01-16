using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;

namespace RoomBooking.Models.Account
{
    public class LogoutViewModel
    {
        private IUserManagementService _userService;
        public void Resolve(IServiceProvider provider)
        {
            _userService = provider.GetRequiredService<IUserManagementService>();
        }
        public async Task LogoutAsync()
        {
            await _userService.LogoutAsync();
        }
    }
}
