using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Room;

namespace RoomBooking.Controllers
{
    [Authorize]
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

        //public async Task<IActionResult> GetRooms()
        //{
        //    TempData.Clear();

        //    try
        //    {
        //        var claim = await GetUserClaim();
        //        var model = new GetAllRoomViewModel();

        //        model.ResolveDI(_provider);
        //        var allRooms = await model.LoadRoomAsync("user");

        //        if(allRooms == null || allRooms.Count == 0)
        //        {
        //            TempData["message"] = "No rooms available now";
        //        }

        //        return View(allRooms);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "There is an exception while performing Get All room operation");
        //        TempData["message"] = "There is an error while load rooms";

        //        return View();
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Get(Guid id)
        //{
        //    TempData.Clear();

        //    try
        //    {
        //        var model = new EditRoomViewModel();
        //        model.ResolveDI(_provider);

        //        model = await model.GetRoomAsync(id);

        //        TempData.Clear();

        //        if (model?.CreatedBy is not null)
        //        {
        //            model.ResolveDI(_provider);

        //            return View(model);
        //        }
        //        else
        //        {
        //            TempData["message"] = "Room doesn't exist . Already deleted";
        //        }

        //        return RedirectToAction("GetRooms");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Room Get operation failed ");
        //        TempData["failure"] = "Room Get operation failed";
        //    }

        //    return RedirectToAction("GetRooms");
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var claim = await GetUserClaim();
                var model = new GetAllRoomViewModel();

                model.ResolveDI(_provider);
                var allRooms = await model.LoadRoomAsync(claim.Item2);

                return View(allRooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is an exception while performing Get All room operation");

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreateRoomViewModel();
                model.ResolveDI(_provider);

                var roomId = Guid.NewGuid();

                var currentURI = $"{HttpContext.Request.Host}/Home/Get/{roomId}";

                model.QRCode = model.QRCodeGeneration(currentURI);
                model.Id = roomId;

                return View(model);
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

            if(user is not null) 
            {
                model.CreatedBy = user.Email;
            }

            if (ModelState.IsValid)
            {
                string response = string.Empty;
                TempData.Clear();

                try
                {
                    model.ResolveDI(_provider);

                    response = await model.CreateRoomAsync(model);

                    if(response.Equals("success"))
                    {
                        TempData["success"] = "Room is Created";
                    }
                    else if (response.Equals("redundant"))
                    {
                        TempData["message"] = "Room already exists ";
                    }
                    else
                    {
                        TempData["message"] = response;
                    }

                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Creating a new room");
                    ModelState.AddModelError(string.Empty, " This room is not created. An Error happended !");

                    return View(model);
                }
            }
            _logger.LogWarning("ModelState is not valid");
            ModelState.AddModelError(string.Empty, "Model State is not valid");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
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

                return RedirectToAction("GetAll");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Room Get operation failed ");
                TempData["failure"] = "Room Get operation failed";
            }

            return RedirectToAction("GetAll");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoomViewModel model)
        {

            if(ModelState.IsValid)
            {
                string response = string.Empty;

                TempData.Clear();

                try
                {
                    model.ResolveDI(_provider);
                    response = await model.EditRoomAsync(model);

                    if (response.Equals("success"))
                    {
                        TempData["success"] = "Room is Updated";
                    }
                    else if (response.Equals("redundant")) 
                    {
                        TempData["message"] = "Room is already exists";
                    }
                    else
                    {
                        TempData["message"] = response;
                    }
                    
                    return RedirectToAction("GetAll");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Updating a new room");
                    TempData["failure"] = "Error in Updating the room";

                    return View(model);
                }
            }
            _logger.LogWarning("ModelState is not valid");
            ModelState.AddModelError(string.Empty, "Model State is not valid");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var model = new DeleteRoomViewModel();
                model.ResolveDI(_provider);

                TempData.Clear();
                string response = string.Empty;

                response = await model.DeleteRoomAsync(id);

                if (response == "success")
                {
                    TempData["success"] = "Room is Deleted";
                }
                else
                {
                    TempData["message"] = response;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Delete operation failed ");
                TempData["failure"] = $"Room can't be deleted, May have events on this room";
            }

            return RedirectToAction("GetAll");
        }

        public async Task<(string, string)> GetUserClaim()
        {
            var user = await _userManager.GetUserAsync(User);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = userClaims.Select(x => x.Value).ToList();
            var claim = string.Empty;

            if (claims.Contains("admin") == true)
            {
                claim = "admin";
            }
            else
            {
                claim = "user";
            }

            return (user.Email, claim);
        }
    }
}