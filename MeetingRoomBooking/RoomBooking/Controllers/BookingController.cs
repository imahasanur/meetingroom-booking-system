using Azure;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Booking;
using RoomBooking.Models.Room;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace RoomBooking.Controllers
{
    [Authorize]
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


        public async Task<IActionResult> NotFound()
        {
            return View();
        }

        public async Task<IActionResult> GetAllRoom()
        {
            var model = new CreateBookingViewModel();
            model.ResolveDI(_provider);
            var rooms = await model.GetAllRoomAsync();
            if(rooms.Count == 0)
            {
                return RedirectToAction("Create");
            }

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

                var user = await GetUserClaim();
                var allUser = _userManager.Users.ToList().Select(x => x.Email).ToList();

                if (user is not (null,null))
                {
                    if(user.Item2 == "user")
                    {
                        model.Host = user.Item1;
                    }

                    model.CreatedBy = user.Item1;
                }

                model.ResolveDI(_provider);

                model.Start = model.Start.AddMinutes(1);

                response = await model.CreateBookingAsync(model, allUser, user.Item2);

                if (response.Equals("success"))
                {
                    TempData["success"] = "Booking is Created";

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
                else if (response.Equals("redundant"))
                {
                    TempData["message"] = "Booking already exists ";

                }
                else
                {
                    TempData["message"] = response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Exception error occured ");
            }

            return Ok();
        }


        [HttpPut]
        public async Task Edit([FromRoute]Guid id, [FromBody] EditBookingViewModel model)
        {
            string response = string.Empty;
            TempData.Clear();

            try
            {
                model.ResolveDI(_provider);

                var user = await GetUserClaim();

                response = await model.EditBookingAsync(model, user.Item1, user.Item2);

                if(response.Equals("success"))
                {
                    TempData["success"] = "Event is Updated";
                    
                }
                else if(response.Equals("redundant"))
                {
                    TempData["message"] = "Event is overlapping ";
                }
                else 
                { 
                    TempData["message"] = response;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error in editing the event on event move action");
                TempData["failure"] = "Error in Updating the Event";
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]Guid id)
        {
            try
            {
                var model = new EditBookingViewModel();

                model.ResolveDI(_provider);
                TempData.Clear();

                model = await model.GetEventByIdAsync(id);

                var allUser = _userManager.Users.ToList().Select(x => x.Email).ToList();

                var user =await GetUserClaim();

                model.UserClaim = user.Item2;

                if(model?.CreatedBy is not null)
                {
                    
                    return View(model);
                }
                else
                {
                    TempData["message"] = "Booking doesn't exist . Already deleted";
                }

                return RedirectToAction("GetAll");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Booking Get operation failed ");
                TempData["failure"] = "Room Booking fetch failed, Occured error";
            }

            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookingViewModel model)
        {
            TempData.Clear();
            string response = string.Empty;

            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogError("Model state is not valid ");
                    return View(model);
                }

                model.ResolveDI(_provider);
                var allUser = _userManager.Users.ToList().Select(x => x.Email).ToList();

                var user = await GetUserClaim();

                var startTime = model.Start;
                var minutes = startTime.Minute;
                var even = minutes % 15 == 0 ? true : false;

                if (even == true)
                {
                    model.Start = model.Start.AddMinutes(1);
                }
                
                response = await model.EditBookingByIdAsync(model, user.Item1, allUser, user.Item2);

                if (response.Equals("success"))
                {
                    TempData["success"] = "Booking is Updated";
                    return RedirectToAction("GetAll");
                }
                else if(response.Equals("not found"))
                {
                    TempData["message"] = "Booking is deleted already ";
                }
                else
                {
                    TempData["message"] = response; 
                }

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There is an exception in meeting edit action");
                TempData["failure"] = "Booking is not updated , an error occured";
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditBooking(Guid Id, string State)
        {
            string response = string.Empty;

            try
            {
                var model = new EditBookingViewModel();

                model.ResolveDI(_provider);
                TempData.Clear();

                model = await model.GetEventByIdAsync(Id);

                var allUser = _userManager.Users.ToList().Select(x => x.Email).ToList();

                var user = await GetUserClaim();

                model.State = State;
                model.ResolveDI(_provider);

                response = await model.EditBookingByIdAsync(model, user.Item1, allUser, user.Item2);

                if (response.Equals("success"))
                {
                    TempData["success"] = "Booking is Updated";
                }
                else if (response.Equals("not found"))
                {
                    TempData["message"] = "Booking is deleted already, Not found ";
                }
                else
                {
                    TempData["message"] = response;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There is an error while tried to update booking state.");
            }

            return RedirectToAction("GetAll");
        }


        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = string.Empty;

            try
            {
                var model = new DeleteBookingViewModel();
                model.ResolveDI(_provider);

                TempData.Clear();

                response = await model.DeleteBookingAsync(id);
                if (response.Equals("success")) 
                { 
                    TempData["success"] = "Booking is Deleted";
                }
                else if(response.Equals("not found"))
                {
                    TempData["message"] = "Can't delete Booking ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete operation failed ");
                TempData["failure"] = "Booking Deletion failed";
            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = new GetAllBookingViewModel();
                model.ResolveDI(_provider);

                var user = await GetUserClaim();

                var allEvent = await model.GetAllEventAsync(user.Item1, user.Item2);

                return View(allEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGuestMeetings()
        {
            try
            {
                var model = new GetAllBookingViewModel();
                model.ResolveDI(_provider);

                var user = await GetUserClaim();

                var allEvent = await model.GetAllGuestEventAsync(user.Item1, user.Item2);

                return View(allEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");

                return View();
            }
        }

        public async Task<(string,string)> GetUserClaim()
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);

            var userClaim = string.Empty;

            if (claims.Count > 1)
            {
                userClaim = "admin";
            }
            else
            {
                userClaim = claims[0].Value;
            }
            return (user.Email, userClaim);
        }
    }
}