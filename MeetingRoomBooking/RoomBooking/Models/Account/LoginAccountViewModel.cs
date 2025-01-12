using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class LoginAccountViewModel
    {
        private IUserManagementService _userService;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }

        public void Resolve(IServiceProvider provider)
        {

            _userService = provider.GetRequiredService<IUserManagementService>();
        }

        public async Task<bool> CheckPreviousLogging(string userEmail)
        {
            bool isTrackingOff = true;
            bool isNewLogIn = false;
            isNewLogIn = await _userService.CheckPreviousLogging(userEmail, isTrackingOff);

            return isNewLogIn;
        }
    }
}
