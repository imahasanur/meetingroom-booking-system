using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Booking;
using System.Runtime.InteropServices;

namespace RoomBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUser _user;
        private readonly IServiceProvider _provider;

        public BookingController(ILogger<BookingController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IServiceProvider provider, ApplicationUser user)
        {
            _logger = logger;
            _signInManager = signInManager;
            _provider = provider;
            _userManager = userManager;
            _user = user;
        }

        public async Task<IActionResult> GetAllRoom()
        {
            var model = new CreateBookingViewModel();
            model.ResolveDI(_provider);
            var rooms = await model.GetAllRoomAsync();

            return Ok(rooms);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            return View();
        }
    }
}
