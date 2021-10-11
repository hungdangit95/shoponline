using System;

namespace ShopOnline.Application.Dapper.ViewModels
{
    public class RevenueReportViewModel
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }

        public decimal Profit { get; set; }
    }
}
