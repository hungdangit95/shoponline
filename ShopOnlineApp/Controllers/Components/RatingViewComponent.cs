using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers.Components
{
    public class RatingViewComponent: ViewComponent
    {
        private readonly IRatingService _ratingService;
        public RatingViewComponent(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productId = HttpContext.Session.Get<int>(CommonConstants.ProductId);
            var dataReturn =await _ratingService.GetAllPaging(new RateRequest
            {
                PageSize = 3,
                PageIndex = 0,
                ProductId = productId
            });

            ViewBag.Ratings = dataReturn.Data.Items.ToList();
            return View();
        }
    }
}
