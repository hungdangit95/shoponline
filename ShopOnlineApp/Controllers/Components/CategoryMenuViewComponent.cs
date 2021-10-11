using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMemoryCache _memoryCache;
        public CategoryMenuViewComponent(IProductCategoryService productCategoryService, IMemoryCache memoryCache)
        {
            _productCategoryService = productCategoryService;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories =await _memoryCache.GetOrCreate(CacheKeys.ProductCategories,
                entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(2);    
                    return _productCategoryService.GetAll();
                });

            return View(categories);
        }
    }

}
