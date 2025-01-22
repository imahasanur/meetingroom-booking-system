using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models;
using RoomBooking.Models.Account;
using RoomBooking.Models.Room;
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
            }
            
            return View();
        }

        public async Task<IActionResult> GetRooms()
        {
            TempData.Clear();

            try
            {
                var model = new GetAllRoomViewModel();

                model.ResolveDI(_provider);
                var allRooms = await model.LoadRoomAsync("user");

                if (allRooms == null || allRooms.Count == 0)
                {
                    TempData["message"] = "No rooms available now";
                }

                return View(allRooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is an exception while performing Get All room operation");
                TempData["message"] = "There is an error while load rooms";

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            TempData.Clear();

            try
            {
                var model = new EditRoomViewModel();
                model.ResolveDI(_provider);

                model = await model.GetRoomAsync(id);

                TempData.Clear();

                if (model?.CreatedBy is not null)
                {
                    model.ResolveDI(_provider);

                    return View(model);
                }
                else
                {
                    TempData["message"] = "Room doesn't exist . Already deleted";
                }

                return RedirectToAction("GetRooms");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Room Get operation failed ");
                TempData["failure"] = "Room Get operation failed";
            }

            return RedirectToAction("GetRooms");
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
