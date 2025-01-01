using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models;
using System.Diagnostics;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            TempData.Clear();

            var user = await _userManager.GetUserAsync(User);

            if(user != null)
            {
                var userClaim = await _userManager.GetClaimsAsync(user);
                var claims = userClaim.Select(x => x.Value).ToList();

                if(claims.Contains("admin") == true)
                {
                    TempData["status"] = $"Welcome to the Admin panel {user.FullName}";
                    TempData["claim"] = "admin";
                }
                else
                {
                    TempData["status"] = $"Welcome to User panel {user.FullName}";
                    TempData["claim"] = "user";
                }
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
