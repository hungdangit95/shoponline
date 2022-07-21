using ShopOnlineApp.Application.ViewModels;
namespace ShopOnlineApp.Models.BlogViewModel
{
    public class BlogCategoryDisplayViewModel : FilterBase<Application.ViewModels.Blogs.BlogViewModel>
    {
        public BlogCategoryViewModel Category { set; get; }
    }
}
