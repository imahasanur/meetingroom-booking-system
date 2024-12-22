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

            var userClaims = users.Select(user => userManager.GetClaimsAsync(user)).ToList();
            var userClaimsResults = await Task.WhenAll(userClaims);

           
            var allAccountModel = new List<GetAllAccountViewModel>();
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                var claims = userClaimsResults[i];
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
