using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Utilities;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class PermissionGrantController : BaseController
    {
        private readonly IBusinessService _businessService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGrantPermissionService _permissionService;
        public PermissionGrantController(IBusinessService businessService, IHttpContextAccessor httpContextAccessor, IUserService userService, IGrantPermissionService permissionService)
        {
            _businessService = businessService;
            _httpContextAccessor = httpContextAccessor;
            _permissionService = permissionService;
        }
        public async Task<IActionResult> Index()
        {
            var lstController = await _businessService.GetAll();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in lstController)
            {
                if (!ExceptController.Except().Contains(item.Id))
                {
                    items.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id
                    });
                }

            }

            ViewBag.items = items;

            ViewBag.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions([FromQuery] string businessId, [FromQuery] Guid userId)
        {
            return new OkObjectResult(await _permissionService.GetPermissons(businessId, userId));
        }

        [HttpGet]
        public async Task<string> UpdatePermission(int id, Guid userId)
        {
            string message = "";
            bool isCheck = await _permissionService.UpdatePermisson(id, userId);
            if (isCheck)
            {
                message = "<div class='alert alert-success'>Quyền cập nhật thành công </div>";
            }
            else
            {
                message = "<div class='alert alert-danger'>Hủy quyền thành công </div>";
            }

            return message;
        }



    }
}