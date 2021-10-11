using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;
using System.Threading.Tasks;

namespace ShopOnlineApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll(RateRequest request)
        {
            var allRates = await _ratingService.GetAllPaging(request);

            return new OkObjectResult(allRates);
        }
        [HttpPost]
        public async Task<IActionResult> Rating(RatingViewModel rate)
        {
            rate.ProductId = HttpContext.Session.Get<int>(CommonConstants.ProductId);
            await _ratingService.Add(rate);
            return new OkObjectResult(rate);
        }
    }
}