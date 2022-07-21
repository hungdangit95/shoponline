using ShopOnline.Application.Dapper.Interfaces;
using ShopOnline.Application.Dapper.ViewModels;
using ShopOnlineApp.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Application.Dapper.Implements
{
    public class ReportRepository : SqlRepositoryBase<RevenueReportViewModel>, IReportRepository
    {
       
    }
}
