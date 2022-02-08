using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Utilities.Mvc.Filters.Exceptions
{
    public class BadRequestException: Exception
    {
       
        public BadRequestException(string messages): base (messages)
        {
        }
    }
}
