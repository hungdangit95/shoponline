using System.Collections.Generic;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
