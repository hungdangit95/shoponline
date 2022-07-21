using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SharedKernel.Extensions;
using ShopOnline.Api.ModelBinders;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopOnline.Api.Servers.Portal
{
    public class HomeController : V1Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IBlogService _blogService;
        private readonly ICommonService _commonService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IProductService productService,
            IBlogService blogService, ICommonService commonService,
            IProductCategoryService productCategoryService, IStringLocalizer<HomeController> localizer, ILogger<HomeController> logger) : base(localizer)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _logger = logger;
        }
        [HttpGet("index")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var homeVm = new HomeViewModel();
            homeVm.HomeCategories = await _productCategoryService.GetHomeCategories(5);
            homeVm.HotProducts = await _productService.GetHotProduct(5);
            homeVm.TopSellProducts = await _productService.GetLastest(5);
            homeVm.NewProducts = await _productService.GetLastest(6);
            homeVm.TopRateProducts = await _productService.GetRatingProducts(3);  
            homeVm.HomeSlides = await _commonService.GetSlides("top");
            _logger.LogError("hung test");
            return Response(Result.Success(homeVm));
        }

        [HttpGet("getbyids/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            return Response(Result.Success(ids));
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
