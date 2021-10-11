using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Data.IRepositories
{
    public interface IProductRepository: IRepository<Product, int>
    {
        Task<IEnumerable<Product>> FindProductsAsync(string name,int page,int pageSize=5);
        Task<IQueryable<Product>> FindAllProductAsync(Expression<Func<Product, bool>> predicate, params Expression<Func<Product, object>>[] includeProperties);
    }
}
