using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class RegisterAccountViewModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IServiceProvider _provider;
        private IUserManagementService _service;

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

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _provider = provider;
            _service = _provider.GetRequiredService<IUserManagementService>();
        }

        public async Task<(IEnumerable<string>? errors, string? message)> RegisterAsync(string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;

            var user = new CreateRegisterUserDTO
            {
                UserName = Email,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                FullName = $"{FirstName} {LastName}",
                Department = Deaprtment,
                MemberPin = MemberPin,
                Phone = Phone,
                Password = Password,
                CreatedAtUtc = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            var result = await _service.RegisterAsync(user); 
            
            return result;
        }

    }
}
