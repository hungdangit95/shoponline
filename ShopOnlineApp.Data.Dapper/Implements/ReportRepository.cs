using ShopOnlineApp.Data.Dapper.Interfaces;
using ShopOnlineApp.Data.Dapper.ViewModels;

namespace ShopOnlineApp.Data.Dapper.Implements
{
    public class ReportRepository : SqlRepositoryBase<RevenueReportViewModel>, IReportRepository
    {

    }
}
