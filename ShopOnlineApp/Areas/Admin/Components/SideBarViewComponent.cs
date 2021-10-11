using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Areas.Admin.Components
{
    public class SideBarViewComponent:ViewComponent
    {
        public readonly IFunctionService _functionService;
        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificDefault("Role");
            List<FunctionViewModel> functions;

            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
                return View(functions);
            }

            //TODO: Get by permission
            if (roles.Length > 4)
            {
                functions = await _functionService.GetFunctionByRoles(new FunctionRequest
                {
                    Roles = roles.Split(";").ToList()
                });

                return View(functions);
            }

            return View(new List<FunctionViewModel>());
        }
    }
}
