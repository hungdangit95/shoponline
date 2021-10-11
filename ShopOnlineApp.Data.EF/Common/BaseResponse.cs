using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnlineApp.Data.EF.Common
{
    public  class BaseReponse<T> where T:class 
    {
        public T Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
