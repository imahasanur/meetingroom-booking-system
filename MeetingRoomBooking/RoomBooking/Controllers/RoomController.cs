using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Room;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Room;

namespace RoomBooking.Controllers
{
    public class RoomController : Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUser _user;
        private readonly IServiceProvider _provider;

        public RoomController( ILogger<RoomController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IServiceProvider provider, ApplicationUser user)
        {
            _logger = logger;
            _signInManager = signInManager;
            _provider = provider;
            _userManager = userManager;
            _user = user;
        }

        public IActionResult Get()
        { 
            return View();
        }

        public IActionResult GetAll()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreateRoomViewModel();
                model.ResolveDI(_provider);
                var rooms = await model.GetAllRoomAsync();

                return View(rooms);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error in Getting All Rooms");

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null) 
            {
                model.CreatedBy = user.Email;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isRepeated = true;

                    model.ResolveDI(_provider);
                    var rooms = await model.GetAllRoomAsync();

                    if(rooms.PreviousRooms is not null)
                    {
                        isRepeated = model.CheckRoomRedundancy(rooms.PreviousRooms, model.Name, model.Location); 
                    }

                    if (isRepeated == true)
                    {
                        ModelState.AddModelError(string.Empty, " This room is already exists !");

                        return View(model);
                    }
                    else
                    {
                        try
                        {
                            await model.CreateRoomAsync(model);
                            TempData["status"] = "Room is Created";

                            return View(model);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error in Creating a new room");
                            ModelState.AddModelError(string.Empty, " This room is not created. An Error happended !");

                            return View(model);
                        }
                    }
 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Creating a new room");
                    ModelState.AddModelError(string.Empty, " An Error is occured!");

                    return View(model);
                }
            }
            _logger.LogWarning("ModelState is not valid");
            ModelState.AddModelError(string.Empty, "Model State is not valid");

            return View(model);
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
