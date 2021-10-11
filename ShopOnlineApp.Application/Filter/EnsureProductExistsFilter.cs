using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopOnlineApp.Application.Interfaces;
using System;

namespace ShopOnlineApp.Application.Filter
{
    public class EnsureProductExistsFilter : IActionFilter
    {
        private readonly IProductService _productService;

        public EnsureProductExistsFilter(IProductService productService)
        {
            _productService = productService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var productId = (int)context.ActionArguments["id"];
            var product = _productService.GetById(productId);
            if(product is null)
            {
                context.Result = new NotFoundResult();
            }
            
        }
    }

    public class EnsureRecipeExistsAttribute : TypeFilterAttribute
    {
        public EnsureRecipeExistsAttribute()
        : base(typeof(EnsureProductExistsFilter)) { }
    }
}
  