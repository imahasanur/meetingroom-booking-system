using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Room;
using RoomBooking.Application.Services.User;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Account
{
    public class CreateAccountViewModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IUserManagementService _userService;

        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
        public IFormFile File { get; set; }

        public void Resolve(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceProvider provider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = provider.GetRequiredService<IUserManagementService>();
        }

        internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegistersAsync(string urlPrefix, List<UserInformation> users)
        {

            ReturnUrl ??= urlPrefix;

            foreach (var user in users)
            {
                var userInformation = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Department = user.Department,
                    PhoneNumber = user.Phone,
                    MemberPin = user.MemberPin,
                    CreatedAtUTC = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                };

                var result = await _userManager.CreateAsync(userInformation, user.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(userInformation, new System.Security.Claims.Claim("role", "user"));

                    var uploadedUser = new CreateUserDTO 
                    {
                        CreatedAtUTC = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        UserEmail = userInformation.Email,
                        IsLoggedIn = false,
                    };

                    await _userService.CreateUploadedUserAsync(uploadedUser); 
                }
                else
                {
                    return (result.Errors, null);
                }
            }

            return (null,ReturnUrl);
        }
    }

    public class UserInformation
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string MemberPin { get; set; }
    }

    public class UserInformationMap : ClassMap<UserInformation>
    {
        public UserInformationMap()
        {
            Map(m => m.Email).Name("Email");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.Password).Name("Password");
            Map(m => m.MemberPin).Name("Pin");
            Map(m => m.Department).Name("Department");
            Map(m => m.Phone).Name("Phone");
        }
    }
}
