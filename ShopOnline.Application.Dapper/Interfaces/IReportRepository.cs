using ShopOnline.Application.Dapper.ViewModels;
using ShopOnlineApp.Data.Dapper.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Application.Dapper.Interfaces
{
    public interface IReportRepository: IRepository<RevenueReportViewModel>
    {

    }
}
