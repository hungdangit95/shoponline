using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Role;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;


        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();

            return new OkObjectResult(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(AppRoleRequest request)
        {
            var model = await _roleService.GetAllPagingAsync(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (!roleVm.Id.HasValue)
            {
                await _roleService.AddAsync(roleVm);
            }
            else
            {
                await _roleService.UpdateAsync(roleVm);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }


        [HttpPost]
        public async Task<IActionResult> ListAllFunction(Guid roleId)
        {
            var functions = await _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            await _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }
    }

}