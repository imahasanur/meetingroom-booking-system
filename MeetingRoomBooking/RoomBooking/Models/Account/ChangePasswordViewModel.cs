using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;
using Sprache;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class ChangePasswordViewModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserManagementService _userService;

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long for pass.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long for pass.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Password", ErrorMessage = "Type properly! It's a mismatch with password")]
        public string ConfirmPassword { get; set; }

        public string? Code { get; set; }

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = provider.GetRequiredService<IUserManagementService>();
        }

        public async Task<(IEnumerable<IdentityError>? errors, bool isChanged)> ChangePassowrdAsync(ChangePasswordViewModel model, ApplicationUser user)
        {
            bool isChanged = false;

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);

                isChanged = true;

                return(null, isChanged);
            }
            else
            {
                return (result.Errors, isChanged);
            }
            
        }
    }
}