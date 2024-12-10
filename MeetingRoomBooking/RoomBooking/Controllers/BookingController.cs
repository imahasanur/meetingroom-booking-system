using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
