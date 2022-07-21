using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Role;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class RoleService : IRoleService
    {
        #region private method
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public RoleService(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork,
            IFunctionRepository functionRepository, IPermissionRepository permissionRepository)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
        }
        #endregion

        #region Public Method

        

        #endregion
        public async Task<bool> AddAsync(AppRoleViewModel roleVm)
        {
            if (!await _roleManager.RoleExistsAsync(roleVm.ToString()))
            {
                await _roleManager.CreateAsync(new AppRole
                {
                    Name = roleVm.Name,
                    NormalizedName = roleVm.ToString().ToUpper()
                });
            }
            //var role = new AppRole()
            //{
            //    Name = roleVm.Name,
            //    Description = roleVm.Description
            //};
            //var result = await _roleManager.CreateAsync(roleVm);
            return true;
        }

        public async Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            //var functions =(await _functionRepository.FindAll()).AsNoTracking();
            //var permissions =(await _permissionRepository.FindAll()).AsNoTracking();
            //var query = from f in functions
            //            join p in permissions on f.Id equals p.FunctionId
            //            join r in _roleManager.Roles on p.RoleId equals r.Id
            //            where roles.Contains(r.Name) && f.Id == functionId
            //            && (p.CanCreate && action == "Create"
            //            || p.CanUpdate && action == "Update"
            //            || p.CanDelete && action == "Delete"
            //            || p.CanRead && action == "Read")
            //            select p;
            //return await query.AnyAsync();
            await Task.CompletedTask;
            return true;
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return new AppRoleViewModel().Map(await _roleManager.Roles.AsNoTracking().ToListAsync()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<AppRoleViewModel>>> GetAllPagingAsync(AppRoleRequest request)
        {
            var query =(await _roleManager.Roles.ToListAsync()).ToList();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText)
                || x.Description.Contains(request.SearchText)).ToList();

            int totalRow = query.Count;

            query = (query.Skip(request.PageIndex * request.PageSize)
               .Take(request.PageSize)).ToList();

            var items = new AppRoleViewModel().Map(query).ToList();

            var result = new BaseReponse<ModelListResult<AppRoleViewModel>>
            {
                Data = new ModelListResult<AppRoleViewModel>()
                {
                    Items = items,
                    Message = Message.Success,
                    RowCount = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };

            return result;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return new AppRoleViewModel().Map(role);
        }

        public async Task<List<PermissionViewModel>>  GetListFunctionWithRole(Guid roleId)
        {
            var functions =(await _functionRepository.FindAll(x => x.Status == Status.Active)).AsNoTracking().AsParallel().AsUnordered();

            var permissions =(await _permissionRepository.FindAll()).AsNoTracking().AsParallel().AsUnordered();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from q in fp.DefaultIfEmpty()
                        where q != null && q.RoleId == roleId
                        select new PermissionViewModel
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanCreate = q != null && q.CanCreate,
                            CanDelete = q != null && q.CanDelete,
                            CanRead = q != null && q.CanRead,
                            CanUpdate = q != null && q.CanUpdate
                        };
            return query.ToList();
        }

        public async Task SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = new PermissionViewModel().Map(permissionVms);

            var oldPermission =(await _permissionRepository.FindAll()).AsParallel().AsOrdered().WithDegreeOfParallelism(2).Where(x => x.RoleId == roleId);

            if (oldPermission.ToList().Any())
            {
               await _permissionRepository.RemoveMultiple(oldPermission.ToList());
            }
            foreach (var permission in permissions)
            {
              await  _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }
    }
}
