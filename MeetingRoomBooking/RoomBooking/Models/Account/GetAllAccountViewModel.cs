using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Evaluation;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Infrastructure.Membership;

namespace RoomBooking.Models.Account
{
    public class GetAllAccountViewModel
    {
        public Guid UserId { get; set; }
        public int ClaimId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }



        public async Task<IList<GetAllAccountViewModel>> GetAllAccountAsync(UserManager<ApplicationUser> userManager)
        {
            var users = userManager.Users.ToList();

            var allAccountModel = new List<GetAllAccountViewModel>();

            foreach (var user in users)
            {
                var claims = await userManager.GetClaimsAsync(user);
                foreach (var claim in claims)
                {
                    allAccountModel.Add(new GetAllAccountViewModel
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.Email,
                        ClaimType = claim.Type ?? string.Empty,
                        ClaimValue = claim.Value ?? string.Empty
                    });
                }
            }

            return allAccountModel;
        }
    }
}
