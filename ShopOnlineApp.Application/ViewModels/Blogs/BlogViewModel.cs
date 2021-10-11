using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.BlogTag;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Blogs
{
    public class BlogViewModel : ViewModelBase<Blog, BlogViewModel>
    {
        public int Id { set; get; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }


        [MaxLength(256)]
        public string Image { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        public string Tags { get; set; }

        public List<BlogTagViewModel> BlogTags { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }

        [MaxLength(256)]
        public string SeoPageTitle { set; get; }

        [MaxLength(256)]
        public string SeoAlias { set; get; }

        [MaxLength(256)]
        public string SeoKeywords { set; get; }

        [MaxLength(256)]
        public string SeoDescription { set; get; }

        public int BlogCategoryId { get; set; }

        public BlogCategoryViewModel BlogCategory { set; get; }

        public int CountComment { get; set; }

    }
}
