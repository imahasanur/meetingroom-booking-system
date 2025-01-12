using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class RegisterAccountViewModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Department")]
        public string Deaprtment { get; set; }

        [Required]
        [StringLength(20,MinimumLength =2)]
        [DataType(DataType.Text)]
        [Display(Name = "Member Pin")]
        public string MemberPin { get; set; }

        public string? ReturnUrl { get; set; }

        public RegisterAccountViewModel() { }

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegisterAsync(string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;

            var user = new ApplicationUser
            {
                UserName = Email,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                FullName = $"{FirstName} {LastName}",
                Department = Deaprtment,
                MemberPin = MemberPin,
                PhoneNumber = Phone,
                CreatedAtUtc = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "user"));
                await _signInManager.SignInAsync(user, isPersistent: false);

                return (null, ReturnUrl);
            }
            else
            {
                return (result.Errors, null);
            }
        }

    }
}
