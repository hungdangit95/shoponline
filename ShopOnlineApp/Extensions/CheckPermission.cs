using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ShopOnlineApp.Extensions
{
    public class CheckPermission: ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string activity = "";
            string controller = "";

            if (string.IsNullOrEmpty(activity))
            {
                activity = ((dynamic)context.ActionDescriptor).ActionName.ToString();
            }

            controller = ((dynamic)context.ActionDescriptor).ControllerName.ToString();

            var actionLink = controller + "Controller-" + activity;
            var serviceProvider = context.HttpContext.RequestServices;

            var permissionService = serviceProvider.GetService<IGrantPermissionService>();
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var userService = serviceProvider.GetService<IUserService>();

            var currentUser = await userManager.FindByIdAsync(userService.GetUserId());

            if (currentUser.UserName== "admin")
            {
                await next();
                return;
            }

            var lstPermission =(await permissionService.GetRoleNameByUserId(currentUser.Id)).ToList();

            if (!lstPermission.Contains(actionLink.ToLower()))
            {
                context.Result = new RedirectResult("/Admin/Authentication/NoAuthenication");
                return;
            }

            await next();
        }
    }
}
