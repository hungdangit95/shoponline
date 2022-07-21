using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Slide;

namespace ShopOnlineApp.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Application.ViewModels.Blogs.BlogViewModel> LastestBlogs { get; set; }
        public List<SlideViewModel> HomeSlides { get; set; }
        public List<ProductViewModel> HotProducts { get; set; }
        public List<ProductViewModel> NewProducts { get; set; }
        public List<ProductViewModel> TopSellProducts { get; set; }
        public  List<ProductViewModel> TopRateProducts { get; set; }
        public List<ProductCategoryViewModel> HomeCategories { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}
