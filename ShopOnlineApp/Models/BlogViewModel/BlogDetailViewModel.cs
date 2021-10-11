using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Models.BlogViewModel
{
    public class BlogDetailViewModel
    {
        public Application.ViewModels.Blogs.BlogViewModel Blog { get; set; }
        
        public IEnumerable<BlogCategoryViewModel>  Categories { get; set; }

        // Popular Post
        public IEnumerable<Application.ViewModels.Blogs.BlogViewModel>  PopularPosts { get; set; }

        //Recent  Comment
        //Relates Post
        public IEnumerable<Application.ViewModels.Blogs.BlogViewModel>  ReLateBlogs { get; set; }
        //Comment
        public IEnumerable<BlogComment> Comments { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<BlogTag> BlogTags { get; set; }
    }
}
