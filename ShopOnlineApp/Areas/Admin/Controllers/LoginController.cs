using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Models.AccountViewModels;
using ShopOnlineApp.Services;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public LoginController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("/admin-login")]
        public async Task<IActionResult> index(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Đăng nhập thành công");
                    return new  OkObjectResult(new GenericResult(true));
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản đã bị khóa");
                    return new OkObjectResult(new GenericResult(false, "Tài khoản đã bị khóa"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không đúng");
                    return new OkObjectResult(new GenericResult(false, "Đăng nhập không đúng"));
                }
            }

            return View(model);
        }
    }
}