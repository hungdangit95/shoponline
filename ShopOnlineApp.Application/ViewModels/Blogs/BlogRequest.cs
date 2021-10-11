using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Blogs
{

    public class BlogRequest : BaseRequest
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }

    }

}
