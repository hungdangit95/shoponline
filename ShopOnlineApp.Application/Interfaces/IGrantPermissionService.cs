using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.GranPermission;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IGrantPermissionService
    {
        Task<List<PermissionActionViewModel>> GetPermissons(string businessId, Guid userId);
        Task<bool> UpdatePermisson(int id, Guid userId);
        Task<IEnumerable<string>> GetRoleNameByUserId(Guid userId);
    }
}
