using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnlineApp.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }

        public IActionResult WishList()
        {
            return ViewComponent("WishList");
        }

        public IActionResult Rating()
        {
            return ViewComponent("Rating");
        }
        public IActionResult BlogComment()
        {
            return ViewComponent("BlogComment");
        }

    }
}