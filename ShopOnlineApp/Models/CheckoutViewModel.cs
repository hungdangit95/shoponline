using System;
using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Application.ViewModels.Bill;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Utilities.Extensions;

namespace ShopOnlineApp.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }
        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                    .Select(c => new EnumModel
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();
            }
        }
        
        public string TypePayment { get; set; }
    }
}
