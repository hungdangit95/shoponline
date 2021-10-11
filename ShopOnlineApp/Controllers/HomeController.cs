using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Models;

namespace ShopOnlineApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBlogService _blogService;
        private readonly ICommonService _commonService;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IProductService productService,
            IBlogService blogService, ICommonService commonService,
            IProductCategoryService productCategoryService, IStringLocalizer<HomeController> localizer)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var title = _localizer["Title"];
            var culture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = await _productCategoryService.GetHomeCategories(5);
            homeVm.HotProducts = await _productService.GetHotProduct(5);
            homeVm.TopSellProducts = await _productService.GetLastest(5);
            homeVm.NewProducts = await _productService.GetLastest(6);
            homeVm.TopRateProducts = await _productService.GetRatingProducts(3);
            homeVm.LastestBlogs = await _blogService.GetLastest(5);
            homeVm.HomeSlides = await _commonService.GetSlides("top");
            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }

}