using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class GrantPermissionRepository : EFRepository<GrantPermission, int>, IGrantPermissionRepository
    {
        public GrantPermissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
