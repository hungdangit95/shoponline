using System;
using System.Collections.Generic;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Product
{
    public class ProductCategoryViewModel:ViewModelBase<ProductCategory,ProductCategoryViewModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int? HomeOrder { get; set; }

        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public List<ProductCategoryViewModel> Children { get; set; }= new  List<ProductCategoryViewModel>();

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        public string SeoPageTitle { set; get; }
        public string SeoAlias { set; get; }
        public string SeoKeywords { set; get; }
        public string SeoDescription { set; get; }

        public ICollection<ProductViewModel> Products { set; get; }
    }
}
