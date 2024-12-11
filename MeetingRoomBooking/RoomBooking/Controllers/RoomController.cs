using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers
{
    public class RoomController : Controller
    {

        public IActionResult Get()
        { 
            return View();
        }

        public IActionResult GetAll()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult ViewAll()
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
    }
}
