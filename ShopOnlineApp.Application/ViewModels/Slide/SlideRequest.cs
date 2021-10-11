using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Slide
{
    public class SlideRequest:BaseRequest
    {
        public string GroupAlias { get; set; }

        public string Name { get; set; }
    }
}
