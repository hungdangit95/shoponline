using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class BlogCommentController : Controller
    {
        private readonly IBlogCommentService _blogCommentService;
        public BlogCommentController(IBlogCommentService blogCommentService)
        {
            _blogCommentService = blogCommentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll(BlogCommentRequest request)
        {
            var allRates = await _blogCommentService.GetAllPaging(request);

            return new OkObjectResult(allRates);
        }
        [HttpPost]
        public async Task<IActionResult> Comment(BlogCommentViewModel rate)
        {
            rate.BlogId = HttpContext.Session.Get<int>(CommonConstants.BlogId);
            await _blogCommentService.Add(rate);
            return new OkObjectResult(rate);
        }
    }
}