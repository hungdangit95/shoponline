using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class ProductRepository : EFRepository<Product, int>, IProductRepository
    {
        private readonly Settings _settings;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public ProductRepository(AppDbContext context, IDistributedCache distributedCache, IConfiguration configuration) : base(context)
        {
            _settings = new Settings(configuration);
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Product>> FindProductsAsync(string name, int page, int pageSize = 5)
        {
            var data = await _context.Products.Where(x => x.Name.Contains(name)).ToListAsync();
            return data?.OrderByDescending(x => x.Price).Skip(page * pageSize).Take(pageSize);
        }

        public async Task<IQueryable<Product>> FindAllProductAsync(Expression<Func<Product, bool>> predicate,
            params Expression<Func<Product, object>>[] includeProperties)
        {
            IEnumerable<Product> products = null;
            string cacheKey = "ProductCache";

            var productEntities = await _distributedCache.GetStringAsync(cacheKey);
            if (productEntities != null)
            {
                //Deserialize
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productEntities);
            }
            else
            {
                products = await _context.Products.AsNoTracking().ToListAsync();

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.PricesExpirationPeriod));
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(products), cacheOptions);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    products = products.AsQueryable().Include(includeProperty).ToList();
                }
            }
            return products.AsQueryable().Where(predicate);
        }

    }
    public class Settings
    {
        public int PricesExpirationPeriod = 15;       //15 minutes by default

        public Settings(IConfiguration configuration)
        {
            int pricesExpirationPeriod;
            if (Int32.TryParse(configuration["Caching:PricesExpirationPeriod"], NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out pricesExpirationPeriod))
            {
                PricesExpirationPeriod = pricesExpirationPeriod;
            }
        }
    }
}
