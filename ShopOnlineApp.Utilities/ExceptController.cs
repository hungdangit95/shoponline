using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ShopOnlineApp.Utilities
{
    public static class ExceptController
    {
        public static List<string> Except()
        {
            return new List<string>()
            {
                "HomeController",
                "BaseController",
                "LoginController",
                "LogoutController",
                "UploadController"
            };
        }
    }
}
