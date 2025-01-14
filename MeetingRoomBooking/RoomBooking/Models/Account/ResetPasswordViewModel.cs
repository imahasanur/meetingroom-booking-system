using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;
using Sprache;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class ResetPasswordViewModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserManagementService _userService;

        public string User { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long for pass.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Type properly! It's a mismatch with password")]
        public string ConfirmPassword { get; set; }

        public string? Code { get; set; }

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = provider.GetRequiredService<IUserManagementService>();
        }

        public async Task<(IEnumerable<IdentityError>? errors, bool isReset)> ResetPassowrdAsync(ResetPasswordViewModel model, ApplicationUser user)
        {
            bool isReset = false;

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                isReset = await _userService.UpdateLoggedInState(user.Email); 

                return(null, isReset);
            }
            else
            {
                return (result.Errors, isReset);
            }
            
        }
    }
}