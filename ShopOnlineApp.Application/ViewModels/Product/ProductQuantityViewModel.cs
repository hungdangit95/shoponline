using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Product
{
    public class ProductQuantityViewModel:ViewModelBase<ProductQuantity,ProductQuantityViewModel>
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public ProductViewModel Product { get; set; }
        public SizeViewModel Size { get; set; }
        public ColorViewModel Color { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime DateModified { get; set; }
    }
}
