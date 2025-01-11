using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class CreateAccountViewModel {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public string? ReturnUrl { get; set; }

        public IFormFile File { get; set; }

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


    }
}
