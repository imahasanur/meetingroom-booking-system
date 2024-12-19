using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using RoomBooking.Application.Domain.Entities;
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

        public async Task<IActionResult> GetAllEvent(DateTime start, DateTime end)
        {

            if (start == DateTime.MinValue)
                start = DateTime.Now;
            if (end == DateTime.MinValue)
                end = DateTime.Now;

            var model = new GetAllBookingViewModel();
            model.ResolveDI(_provider);

            var allEvent = await model.GetAllEventAsync(start, end);
          

            return Ok(allEvent);
        }

        public async Task<IActionResult> Create()
        {
            TempData.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingViewModel model)
        {

            string response = string.Empty;
            try
            {
                TempData.Clear();
                model.Id = Guid.NewGuid();

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Model State is not valid for Booking Create Action");
                    TempData["message"] = "Model State is not valid";

                    return View();
                }

                var user = await _userManager.GetUserAsync(User);

                if (user is not null)
                {
                    model.CreatedBy = user.Email;
                }

                model.ResolveDI(_provider);

                response = await model.CreateBookingAsync(model);

                if (response.Equals("success"))
                {
                    TempData["success"] = "Booking is Created";
                }
                else if (response.Equals("redundant"))
                {
                    TempData["message"] = "Booking already exists ";
                }
                var newEvent = new ScheduleEvent
                {
                    Id = model.Id,
                    Text = model.Name,
                    Start = model.Start,
                    End = model.End,
                    Resource = model.RoomId,
                    Color = model.Color,
                };

                return Ok(newEvent);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Exception error occured ");
            }

            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromRoute]Guid id, [FromBody] EditBookingViewModel model)
        {

            string response = string.Empty;

            TempData.Clear();

            try
            {
                model.ResolveDI(_provider);
                response = await model.EditBookingAsync(model);

                if (response.Equals("success"))
                {
                    TempData["success"] = "Event is Updated";
                }
                else if (response.Equals("redundant"))
                {
                    TempData["message"] = "Event is overlapping ";
                }
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error in editing the event on event move action");
                TempData["failure"] = "Error in Updating the Event";
                return BadRequest();
            }
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
