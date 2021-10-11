using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ShopOnline.Application.Dapper.Interfaces;
using ShopOnline.Application.Dapper.ViewModels;

namespace ShopOnline.Application.Dapper.Implements
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;

        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate)
        {
            return null;
            //using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            //{
            //    await sqlConnection.OpenAsync();
            //    var dynamicParameters = new DynamicParameters();
            //    var now = DateTime.Now;

            //    var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            //    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //    dynamicParameters.Add("@fromDate", string.IsNullOrEmpty(fromDate) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate);
            //    dynamicParameters.Add("@toDate", string.IsNullOrEmpty(toDate) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate);

            //    try
            //    {
            //        return await sqlConnection.QueryAsync<RevenueReportViewModel>(
            //            "GetRevenueDaily", dynamicParameters, commandType: CommandType.StoredProcedure);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}
        }
    }
}
