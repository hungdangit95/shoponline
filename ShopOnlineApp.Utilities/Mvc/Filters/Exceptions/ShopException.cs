﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnlineApp.Utilities.Mvc.Filters
{
    public class ShopException : Exception
    {
        public ShopException(string message): base(message)
        {

        }
    }
}
