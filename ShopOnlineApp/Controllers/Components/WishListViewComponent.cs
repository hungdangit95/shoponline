using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopOnlineApp.Models;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers.Components
{
    public class WishListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var session = HttpContext.Session.GetString(CommonConstants.WishListSession);
            var cart = new List<WishListViewModel>();
            if (session != null)
                cart = JsonConvert.DeserializeObject<List<WishListViewModel>>(session);
            ViewBag.CountWishList = cart.Count;
            return View();
        }
    }
}
