using CsvHelper.TypeConversion;
using CsvHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Models.Account;
using System.Globalization;
using System.Security.Claims;
using CsvHelper.Configuration;
using System.Text;

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
                var allUser = await model.LoadAccountAsync(_userManager);

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

        [HttpGet]
        public async Task<IActionResult> Create()
        
        {
            var model = new CreateAccountViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Choose the csv file");

                return View(model);
            }

            if (model.File != null && model.File.Length > 0)
            {
                
                try
                {
                    var users = new List<UserInformation>();
                    using (var dataStream = model.File.OpenReadStream())
                    {
                        users = ReadCsvFile(dataStream).ToList();
                    }

                    if (users.Count > 0)
                    {
                        var invalidRecord = users.Where(x => x.FirstName == string.Empty || x.LastName == string.Empty || x.Email == string.Empty || x.Password == string.Empty).ToList();
                            
                        if (invalidRecord.Count > 0)
                        {
                            ModelState.AddModelError(string.Empty, "Csv file contains Null fields value");
                                
                            return View(model);
                        }

                        model.Resolve(_userManager, _signInManager, _provider);
                        var response = await model.RegistersAsync(Url.Content("~/"), users);

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
                        
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    _logger.LogError(ex, "There is an ApplicationException");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                    _logger.LogError(ex, $"There is an exception {ex.Message}");
                }
            }

            return View(model);
        }

        public IEnumerable<UserInformation> ReadCsvFile(Stream fileStream)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                    Delimiter = ",",
                    Quote = '\'',
                    Encoding = Encoding.UTF8,
                    MissingFieldFound = null,
                    MemberTypes = MemberTypes.Fields
                };

                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, config))
                {
                    // Register mapping for properties.
                    // Remove Error CsvHelper.ReaderException: 'No members are mapped for type.
                    csv.Context.RegisterClassMap<UserInformationMap>(); 
                    var records = csv.GetRecords<UserInformation>().ToList();

                    return records.ToList();
                }
            }
            catch (HeaderValidationException ex)
            {
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }
    }
}