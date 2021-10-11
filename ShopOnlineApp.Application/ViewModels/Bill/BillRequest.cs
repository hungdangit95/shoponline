using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Bill
{
    public class BillRequest:BaseRequest
    {
        public string StartDate { get; set; }
       public string EndDate { get; set; }
    }
}
