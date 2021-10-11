using System;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Data.IRepositories
{
    public interface IAppUserRoleRepository : IRepository<AppRole, Guid>
    {
    }
}
