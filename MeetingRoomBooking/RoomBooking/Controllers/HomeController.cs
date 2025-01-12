using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models;
using RoomBooking.Models.Account;
using System.Diagnostics;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceProvider _provider;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceProvider provider)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _provider = provider;
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

                var model = new LoginAccountViewModel();

                model.Resolve(_provider);

                var isPreviousLoggedIn = await model.CheckPreviousLogging(user.Email);

                if (claims.Contains("admin") == true)
                {
                    TempData["status"] = $"Welcome to the Admin panel {user.FullName}";
                    TempData["claim"] = "admin";
                }
                else
                {
                    TempData["status"] = $"Welcome to User panel {user.FullName}";
                    TempData["claim"] = "user";
                }

                if (isPreviousLoggedIn == false)
                {
                    return RedirectToAction("ResetPassword", "Account");
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
