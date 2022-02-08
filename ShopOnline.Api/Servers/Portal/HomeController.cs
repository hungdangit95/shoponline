using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Api.Servers.Portal
{
    public class HomeController : V1Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBlogService _blogService;
        private readonly ICommonService _commonService;
       

        public HomeController(IProductService productService,
            IBlogService blogService, ICommonService commonService,
            IProductCategoryService productCategoryService, IStringLocalizer<HomeController> localizer):base(localizer)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {

            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = await _productCategoryService.GetHomeCategories(5);
            homeVm.HotProducts = await _productService.GetHotProduct(5);
            homeVm.TopSellProducts = await _productService.GetLastest(5);
            homeVm.NewProducts = await _productService.GetLastest(6);
            homeVm.TopRateProducts = await _productService.GetRatingProducts(3);
            homeVm.LastestBlogs = await _blogService.GetLastest(5);
            homeVm.HomeSlides = await _commonService.GetSlides("top");
            return Ok(homeVm);
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";
        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //[HttpPost]
        //public IActionResult SetLanguage(string culture, string returnUrl)
        //{
        //    Response.Cookies.Append(
        //        CookieRequestCultureProvider.DefaultCookieName,
        //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        //    );

        //    return LocalRedirect(returnUrl);
        //}
    }
}
