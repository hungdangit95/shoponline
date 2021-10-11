using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Application.Dapper.Interfaces;
using ShopOnlineApp.Extensions;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
   
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;
        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            var getInfo = User.GetSpecificDefault("Email");
            return View();
        }
        public async Task<IActionResult> GetRevenue(string fromDate, string toDate)
        {
            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
            }
            return new OkObjectResult(new{});
        }
    }
}