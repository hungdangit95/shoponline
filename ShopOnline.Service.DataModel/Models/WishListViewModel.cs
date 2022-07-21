using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;

namespace ShopOnlineApp.Models
{
    public class WishListViewModel
    {
        public ProductViewModel Product { set; get; }

        public int Quantity { get; set; }
    }
}
