using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Bill
{
    public class BillDetailViewModel:ViewModelBase<BillDetailViewModel,BillDetail>
    {
        public int Id { get; set; }

        public int BillId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public BillViewModel Bill { set; get; }
        public ProductViewModel Product { set; get; }
        public ColorViewModel Color { set; get; }

        public SizeViewModel Size { set; get; }
    }
}
