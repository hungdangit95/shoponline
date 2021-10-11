using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class PermissionRepository:EFRepository<Permission,int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
