using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class AuthenticationController : BaseController
    {
      
        public IActionResult NoAuthenication()
        {
            return View();
        }

    }
}