using Microsoft.AspNetCore.Identity;
using RoomBooking.Infrastructure.Membership;
using System.Security.Claims;

namespace RoomBooking.Models.Account
{
    public class EditAccountViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public async Task<IList<EditAccountViewModel>> GetUserClaimAsync(Guid id, UserManager<ApplicationUser> userMangaer)
        {
            var user = userMangaer.Users.FirstOrDefault(x => x.Id == id);
            var userAllClaim = new List<EditAccountViewModel>();
            if (user is not null)
            {
                var userClaim =await userMangaer.GetClaimsAsync(user);
                if (userClaim != null) { 
                    foreach(var claim in userClaim)
                    {
                        var aClaim = new EditAccountViewModel
                        {
                            UserId = user.Id,
                            ClaimType = claim.Type ?? string.Empty,
                            ClaimValue = claim.Value?? string.Empty,
                            UserName = user.Email ?? string.Empty,
                        };
                        userAllClaim.Add(aClaim);
                    }
                }

            }
            return userAllClaim;
        }

        public async Task<string> EditUserClaimAsync(UserManager<ApplicationUser> userManager, EditAccountViewModel model)
        {
            string response = string.Empty;

            var user = await userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                response = "not found";
                return response;
            }

            var existingClaims = await userManager.GetClaimsAsync(user);

            foreach (var claim in existingClaims)
            {
                if (claim.Type == "role" && (claim.Value == "admin" || claim.Value == "user"))
                {
                    await userManager.RemoveClaimAsync(user, claim);
                }
            }

            if (!string.IsNullOrWhiteSpace(model.ClaimValue))
            {
                var newClaim = new Claim("role", model.ClaimValue);
                await userManager.AddClaimAsync(user, newClaim);
            }

            response = "success";

            return response;
        }
    }
}
