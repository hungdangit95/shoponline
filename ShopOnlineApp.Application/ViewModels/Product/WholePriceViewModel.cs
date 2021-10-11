using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Product
{
    public class WholePriceViewModel:ViewModelBase<WholePrice,WholePriceViewModel>
    {
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }
    }
}
