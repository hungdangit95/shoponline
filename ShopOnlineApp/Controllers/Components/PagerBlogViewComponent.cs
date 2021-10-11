using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Controllers.Components
{
    public class PagerBlogViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModelListResult<BlogViewModel> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
