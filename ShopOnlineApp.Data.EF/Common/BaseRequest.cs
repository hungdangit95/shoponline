using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnlineApp.Data.EF.Common
{
    public  class BaseRequest
    {
        #region Public Properties
        public string SearchText { get; set; }
        public string OrderBy { get; set; }
        public string Direction { get; set; } = "asc";
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public bool IsPaging => PageSize > 0;

        public IList<KeyValuePair<string, bool>> Order
        {
            get
            {
                if (string.IsNullOrEmpty(OrderBy)) return null;
                return new[] { new KeyValuePair<string, bool>(OrderBy, Direction == "asc") };
            }
        }

        #endregion Public Properties


    }
}
