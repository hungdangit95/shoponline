using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace ShopOnlineApp.Controllers.Components
{
    public class BrandViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
