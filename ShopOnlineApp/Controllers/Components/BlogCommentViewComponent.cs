using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers.Components
{
    public class BlogCommentViewComponent:ViewComponent
    {
        private readonly IBlogCommentService _blogCommentService;
        public BlogCommentViewComponent(IBlogCommentService blogCommentService)
        {
            _blogCommentService = blogCommentService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogId = HttpContext.Session.Get<int>(CommonConstants.BlogId);
            var dataReturn =await _blogCommentService.GetAllPaging(new BlogCommentRequest
            {
                PageSize = 5,
                PageIndex = 0,
                BlogId = blogId
            });

            ViewBag.Comments = dataReturn.Data.Items.ToList();

            return View();
        }
    }
}
