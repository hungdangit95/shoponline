using System;
using System.Collections.Generic;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Data.EF.Common
{
    public class ModelListResult<T> where T:class
    {
        public ModelListResult()
        {
            Results = new List<T>();
        }
        public IList<T> Results { get; set; }
        public IList<T> Items { get; set; }
   
        public int PageIndex { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                PageCount = value;
            }
        }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
        public int CurrentPage { get; set; }

        public int FirstRowOnPage => (PageIndex - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(PageIndex * PageSize, RowCount);

        public string Message { get; set; }
        public QueryStatus Status { get; set; }
    }
}
