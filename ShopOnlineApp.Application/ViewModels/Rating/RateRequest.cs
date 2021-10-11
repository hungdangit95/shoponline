using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Rating
{
    public class RateRequest:BaseRequest
    {
        public int ProductId { get; set; }
    }
}
