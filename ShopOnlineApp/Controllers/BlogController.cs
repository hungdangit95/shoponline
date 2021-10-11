using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Models.BlogViewModel;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;
namespace ShopOnlineApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IBlogCommentRepository _blogComment;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IConfiguration _configuration;

        public int Count { get; set; } = 0;

        public BlogController(IBlogService blogService, IBlogCategoryService blogCategoryService, IBlogCommentRepository blogComment, IBlogTagRepository blogTagRepository, IConfiguration configuration)
        {
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _blogComment = blogComment;
            _blogTagRepository = blogTagRepository;
            _configuration = configuration;
        }

        [Route("{alias}-b.{id}.html", Name = "BlogDetail")]
        public async Task<IActionResult> Index(int id)
        {
            var model = new BlogDetailViewModel();
            model.Blog =await _blogService.GetById(id);
            model.Blog.CountComment =(await _blogComment.FindAll()).Count(x => x.BlogId == id);
            model.Categories = await _blogCategoryService.GetAll();
            model.PopularPosts = (from k in (await _blogService.GetAll()).OrderByDescending(x => x.DateModified).ToList()
                                  select new BlogViewModel
                                  {
                                      Image = k.Image,
                                      Id = k.Id,
                                      CountComment = 4,
                                      DateModified = k.DateModified,
                                      Description = k.Description,
                                      SeoAlias = k.SeoAlias
                                  }).Take(5);
            model.ReLateBlogs = (from b in (await _blogService.GetAll()).Where(x => x.BlogCategoryId == model.Blog.BlogCategoryId && x.Id != id ).ToList()
                                 select new BlogViewModel
                                 {
                                     Image = b.Image,
                                     Id = b.Id,
                                     CountComment=0,
                                   //  CountComment =(await _blogComment.FindAll(y => y.BlogId == b.Id)).Count(),
                                     DateModified = b.DateModified,
                                     Description = b.Description,
                                     SeoAlias = b.SeoAlias
                                 }).Take(5);

            model.BlogTags =(await _blogTagRepository.FindAll()).Take(10);

            HttpContext.Session.Set(CommonConstants.BlogId, id);
            model.Tags =(await _blogTagRepository.FindAll(x => x.BlogId == id)).Select(x => x.TagId).ToList();
            return View(model);
        }

        [Route("blogs.html")]
        public async Task<IActionResult> Content( int? pageSize, string sortBy, int page = 1)
        {
            var request = new BlogRequest();
            var blogs = new BlogFilterViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            request.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            blogs.PageSize = request.PageSize;
            request.SortBy = sortBy;
            request.PageIndex = page;
            blogs.SortType = sortBy;
            var data = await _blogService.GetAllPaging(request);
            blogs.Data = data.Data;
            return View(blogs);
        }

        [Route("{alias}-c-{id}.html")]
        public async Task<IActionResult> Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var request = new BlogRequest();
            var catalog = new BlogCategoryDisplayViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            request.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            request.CategoryId = id;
            catalog.PageSize = request.PageSize;
            request.SortBy = sortBy;
            request.PageIndex = page;
            catalog.SortType = sortBy;
            var data = await _blogService.GetAllPaging(request);
            catalog.Data = data.Data;
            catalog.Category =await _blogCategoryService.GetById(id);
            return View(catalog);
        }
    }
}