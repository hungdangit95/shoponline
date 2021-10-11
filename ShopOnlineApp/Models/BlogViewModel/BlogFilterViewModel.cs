using ShopOnlineApp.Application.ViewModels;

namespace ShopOnlineApp.Models.BlogViewModel
{
    public class BlogFilterViewModel:FilterBase<Application.ViewModels.Blogs.BlogViewModel>
    {
        public BlogCategoryViewModel Category { set; get; }
    }
}
