using System.Collections.Generic;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Common
{
    public class PagedResult<T> : ModelListResult<T> where T : class
    {
        public PagedResult()
        {
            Results = new List<T>();
        }
    }

}
