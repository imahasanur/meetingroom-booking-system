using Microsoft.AspNetCore.Mvc;
using RoomBooking.Models;

namespace RoomBooking.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegistrationModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationModel model)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
