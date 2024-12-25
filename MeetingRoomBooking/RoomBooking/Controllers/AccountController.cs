﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Account;
using System.Security.Claims;

namespace RoomBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _provider;

        public AccountController(ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IServiceProvider provider)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _provider = provider;
        }
        public IActionResult Register()
        {
            var model = new RegisterAccountViewModel();
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_userManager, _signInManager);
                var response = await model.RegisterAsync(Url.Content("~/"));

                if (response.errors is not null)
                {
                    foreach (var error in response.errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError(error.Description);
                    }
                }
                else
                {
                    return Redirect(response.redirectLocation);
                }    
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var model = new LoginAccountViewModel 
            { 
                ReturnUrl = returnUrl 
            };

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAccountViewModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var claims = (await _userManager.GetClaimsAsync(user)).ToArray();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            _logger.LogInformation("Model State is not valid");

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");

                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            { 
                _logger.LogWarning($"Error in the redirect to: {returnUrl}");
                return RedirectToAction("AccessDenied");
            }

        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var model = new GetAllAccountViewModel();

                var allUser = await model.GetAllAccountAsync(_userManager);
                return View(allUser);   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "There is an error while getting all users from Identity");
            }
           
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new EditAccountViewModel();

            try
            {
                var newModel = await model.GetUserClaimAsync(id, _userManager);

                return View(newModel);
            }
            catch (Exception ex) { 
                _logger.LogError($"Error: {ex.Message}");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["failure"] = "Model State is not valid";
                return View(model);
            }

            var response = string.Empty;
            TempData.Clear();

            try
            {
                response = await model.EditUserClaimAsync(_userManager, model);

                if(response == "success")
                {
                    TempData["success"] = "Account Role value updated successfully ";
                }
                else
                {
                    TempData["failure"] = response;
                }
                
                return RedirectToAction("GetAll");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating account: {ex.Message}");
                TempData["failure"] = "An error occurred while updating the account claims.";

                return RedirectToAction("GetAll");
            }
        }

    }
}