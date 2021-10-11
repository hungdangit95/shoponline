using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.BlogComment
{
    public class BlogCommentRequest:BaseRequest
    {
        public int BlogId { get; set; }
    }
}
