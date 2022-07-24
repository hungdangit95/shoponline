using ShopOnlineApp.Data.Dapper.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
