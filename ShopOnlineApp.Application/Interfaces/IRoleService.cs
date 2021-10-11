using ShopOnlineApp.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel userVm);
        Task DeleteAsync(Guid id);
        Task<List<AppRoleViewModel>> GetAllAsync();
        Task<BaseReponse<ModelListResult<AppRoleViewModel>>> GetAllPagingAsync(AppRoleRequest request);
        Task<AppRoleViewModel> GetById(Guid id);
        Task UpdateAsync(AppRoleViewModel userVm);
        Task<List<PermissionViewModel>>  GetListFunctionWithRole(Guid roleId);
        Task SavePermission(List<PermissionViewModel> permissions, Guid roleId);
        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
