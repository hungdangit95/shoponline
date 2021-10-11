using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.GranPermission;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class GrantPermissionService : IGrantPermissionService
    {

        private readonly IGrantPermissionRepository _grantPermissionRepository;
        private readonly IBusinessActionRepository _businessActionService;
        private readonly IUnitOfWork _unitOfWork;

        public GrantPermissionService(IGrantPermissionRepository grantPermissionRepository, IBusinessActionRepository businessActionService, IUnitOfWork unitOfWork)
        {
            _grantPermissionRepository = grantPermissionRepository;
            _businessActionService = businessActionService;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PermissionActionViewModel>> GetPermissons(string businessId, Guid userId)
        {

            var grantItems = (from g in (await _grantPermissionRepository.FindAll()).AsNoTracking()
                              join p in await _businessActionService.FindAll() on g.BusinessActionId equals p.Id
                              where g.UserId == userId && p.BusinessId == businessId
                              select new PermissionActionViewModel
                              {
                                  Description = p.Description,
                                  IsGranted = true,
                                  PermissionID = p.Id,
                                  PermissionName = p.Name
                              });

            var allPermissons = from p in (await _businessActionService.FindAll()).AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3).GroupBy(x => x.Name).Select(x => x.FirstOrDefault())
                                where p.BusinessId == businessId
                                select new PermissionActionViewModel
                                { PermissionID = p.Id, PermissionName = p.Name, Description = p.Description, IsGranted = false };

            var listpermissionId = grantItems.Select(p => p.PermissionID);


            foreach (var item in allPermissons)
            {
                if (!listpermissionId.Contains(item.PermissionID))
                    grantItems.ToList().Add(item);
            }

            return grantItems.ToList();
        }
        public async Task<bool> UpdatePermisson(int id, Guid userId)
        {
            var grant = (await _grantPermissionRepository.FindAll(x => x.BusinessActionId == id)).AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3).SingleOrDefault();
            if (grant == null)
            {
                var shopGrant = new GrantPermission
                {
                    BusinessActionId = id,
                    UserId = userId
                };

                await _grantPermissionRepository.Add(shopGrant);
                _unitOfWork.Commit();

                return true;
            }
            await _grantPermissionRepository.Remove(grant);
            _unitOfWork.Commit();
            return false;
        }


        public async Task<IEnumerable<string>> GetRoleNameByUserId(Guid userId)
        {
            var permissions = from p in (await _businessActionService.FindAll()).AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3).ToList()
                              join g in (await _grantPermissionRepository.FindAll()).AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3)
                                  on p.Id equals g.BusinessActionId
                              where g.UserId == userId
                              select p.Name.ToLower();
            return permissions;
        }
    }
}
