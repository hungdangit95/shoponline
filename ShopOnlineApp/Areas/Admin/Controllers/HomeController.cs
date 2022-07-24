using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
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
            fromDate = "02/10/2019";
            toDate = "02/16/2019";
            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
            }
            return new OkObjectResult(new{});
        }
    }
}