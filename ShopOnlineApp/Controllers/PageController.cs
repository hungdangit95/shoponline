using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;

namespace ShopOnlineApp.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [Route("page/{alias}.html", Name = "Page")]
        public async Task<IActionResult> Index(string alias)
        {
            var page = await _pageService.GetByAlias(alias);
            return View(page);
        }
    }
}