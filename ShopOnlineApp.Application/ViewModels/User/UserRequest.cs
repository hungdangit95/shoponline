using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.User
{
    public class UserRequest : BaseRequest
    {
        public string  CategoryId { get; set; }
    }
}
