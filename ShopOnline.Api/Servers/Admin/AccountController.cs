using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Localization;
using SharedKernel.Extensions;
using ShopOnline.Api.ActionFilter;
using ShopOnline.Api.MiddleWare;
using ShopOnline.Api.Servers.Portal;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Authentication;
using System.Threading.Tasks;

namespace ShopOnline.Api.Servers.Admin
{
    public class AccountController : V1Controller
    {
        private readonly IOAuthenticationService _authenticationService;
        public AccountController(IStringLocalizer<AccountController> localizer, IOAuthenticationService authenticationService) : base(localizer)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationViewModel userForRegistration)
        {
            var result = await _authenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    return Response(Result.Fail("Register failure"));
                }
            }
            return Response(Result.Success());
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationViewModel user)
        {
            if (!await _authenticationService.ValidateUser(user))
                return Unauthorized();
            return Ok(new
            {
                Token = await _authenticationService.CreateToken()
            });
        }

        [Authorize]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }

    }
}
