using Dapper;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Data.Dapper.Interfaces;
using ShopOnlineApp.Data.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShopOnlineApp.Application.Implementation
{
    public class ReportService : IReportService
    {
        
        private readonly IConfiguration _configuration;
        private readonly IReportRepository _reportRepository;
        public ReportService(IConfiguration configuration, IReportRepository reportRepository)
        {
            _configuration = configuration;
            _reportRepository = reportRepository;
        }
        public async Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate)
        {

            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dynamicParameters.Add("@fromDate", string.IsNullOrEmpty(fromDate) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate);
                dynamicParameters.Add("@toDate", string.IsNullOrEmpty(toDate) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate);

                try
                {
                    var response  = await _reportRepository.ExcuteManyAsync("dbo", "GetRevenueDaily", dynamicParameters);
                    return response;
                    //return await sqlConnection.QueryAsync<RevenueReportViewModel>("GetRevenueDaily", dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
