using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ShopOnline.Share.ChainableResultExtension;

namespace ShopOnline.Api.Servers.Portal
{
    [ApiController, Route("api/v1/[controller]")]
    public class V1Controller : ControllerBase
    {
        protected readonly IStringLocalizer _localizer;

        public V1Controller(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            switch (error)
            {
                case string message:
                    {
                        var msg = _localizer[message];
                        ModelState.AddModelError(string.Empty, msg);
                        return new BadRequestObjectResult(new ValidationProblemDetails(ModelState)
                        {
                            Detail = msg,
                            Title = msg
                        });
                    }

                case LocalizedString localizedString:
                    {
                        var message = localizedString.Value;
                        ModelState.AddModelError(string.Empty, message);
                        return new BadRequestObjectResult(new ValidationProblemDetails(ModelState)
                        {
                            Detail = message,
                            Title = message
                        });
                    }

                case Result firebaseResult:
                    {
                        var msg = _localizer[firebaseResult.Error];
                        ModelState.AddModelError(string.Empty, msg);
                        return new BadRequestObjectResult(new ValidationProblemDetails(ModelState)
                        {
                            Detail = msg,
                            Title = msg
                        });
                    }

                default:
                    return base.BadRequest(error);
            }
        }

        protected IActionResult Result<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(_localizer[result.Error]);
        }

        protected IActionResult Result(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(_localizer[result.Error]);
        }
    }
}
