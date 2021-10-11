using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnline.Application.Dapper.ViewModels;

namespace ShopOnline.Application.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
