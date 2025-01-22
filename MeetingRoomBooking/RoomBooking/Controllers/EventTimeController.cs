using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Room;
using RoomBooking.Data.Migrations;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.EventTime;
using RoomBooking.Models.Room;

namespace RoomBooking.Controllers
{
    [Authorize]
    public class EventTimeController : Controller
    {
        private readonly ILogger<EventTimeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUser _user;
        private readonly IServiceProvider _provider;

        public EventTimeController( ILogger<EventTimeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IServiceProvider provider, ApplicationUser user)
        {
            _logger = logger;
            _signInManager = signInManager;
            _provider = provider;
            _userManager = userManager;
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var model = new EditEventTimeViewModel();

            try
            {
                model.ResolveDI(_provider);

                model = await model.GetEventTimeLimitAsync();

                TempData.Clear();

                if (model?.MinimumTime != 0)
                { 
                    return View(model);
                }
                else
                {
                    TempData["message"] = "Time Limit doesn't exist";
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Meeting Time limit Get operation failed ");
                TempData["failure"] = "Meeting Time limit Get operation failed";
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEventTimeViewModel model)
        {
            var response = string.Empty;

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Model state is not valid in EventTime Edit action");
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);

                if (user is not null)
                {
                    model.UpdatedBy = user.Email;
                }

                model.ResolveDI(_provider);
                response = await model.EditEventTimeLimitAsync(model);

                TempData.Clear();

                if(response == "success")
                {
                    TempData["success"] = "Event Time limit has updated";
                    return View(model);
                }
                else
                {
                    TempData["message"] = response;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Meeting Time limit Get operation failed ");
                TempData["failure"] = "Meeting Time limit set operation failed";
            }

            return View(model);
        }
    }
}