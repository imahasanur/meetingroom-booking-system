using Microsoft.AspNetCore.Identity;
using RoomBooking.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RoomBooking.Models.Account
{
    public class EditAccountViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ClaimType { get; set; }
        public List<string> ClaimValue { get; set; }

        [Required]
        public string NewClaimValue { get; set; }

        public async Task<EditAccountViewModel> GetUserClaimAsync(Guid id, UserManager<ApplicationUser> userMangaer)
        {
            var user = userMangaer.Users.FirstOrDefault(x => x.Id == id);
            var userAllClaim = new EditAccountViewModel();
            
            if(user is not null)
            {
                var userClaim =await userMangaer.GetClaimsAsync(user);
                if(userClaim != null && userClaim.Count > 0) 
                {
                    var claims = new List<string>();

                    for(int i = 0; i < userClaim.Count; i++)
                    {
                        if(i == 0)
                        {
                            userAllClaim.UserId = user.Id;
                            userAllClaim.UserName = user.Email ?? string.Empty;
                            userAllClaim.ClaimType = userClaim[0].Type ?? string.Empty;
                            claims.Add(userClaim[0].Value?? string.Empty);
                            continue;
                        }

                        claims.Add(userClaim[i].Value ?? string.Empty);
                    }
                    userAllClaim.ClaimValue = claims;
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

            if (!string.IsNullOrWhiteSpace(model.NewClaimValue))
            {
                if(NewClaimValue.Length <= 5)
                {
                    var newClaim = new Claim("role", model.NewClaimValue);
                    await userManager.AddClaimAsync(user, newClaim);
                }
                else
                {
                    var newClaimValue = model.NewClaimValue.Split(',');
                    foreach(var claim in newClaimValue)
                    {
                        var aClaim = new Claim("role", claim);
                        await userManager.AddClaimAsync(user, aClaim);
                    }
                }
            }

            response = "success";

            return response;
        }
    }
}
