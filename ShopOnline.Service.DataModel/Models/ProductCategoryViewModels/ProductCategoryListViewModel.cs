using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;

namespace ShopOnlineApp.Models.ProductCategoryViewModels
{
    public class ProductCategoryListViewModel
    {
        public LookupItem CurrentCategory { get; set; }
        public List<LookupItem>  Categories { get; set; }
        public List<LookupItem> Colors { get; set; }
        public List<LookupItem> Sizes { get; set; }
        public List<ShoppingCartViewModel> CartViewModels { get; set; }= new List<ShoppingCartViewModel>();
    }
}
