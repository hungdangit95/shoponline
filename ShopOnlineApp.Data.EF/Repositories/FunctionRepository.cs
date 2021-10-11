using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class FunctionRepository: EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
