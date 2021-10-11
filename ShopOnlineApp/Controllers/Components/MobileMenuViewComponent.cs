using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;

namespace ShopOnlineApp.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {

        private readonly IProductCategoryService _productCategoryService;

        public MobileMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _productCategoryService.GetAll());
        }
    }
}
