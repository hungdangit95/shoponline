using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.EF
{
    public class EFRepository<T, K> : IRepository<T, K>, IDisposable where T : DomainEntity<K>
    {
        protected AppDbContext _context;

        public EFRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentException(null, nameof(context)); ;
        }
        public async Task Add(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<IQueryable<T>> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            var items = await GetAll().ToListAsync();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.AsQueryable().Include(includeProperty).ToList();
                }
            }
            return items.AsQueryable();
        }

        public async Task<IQueryable<T>> FindAll(Expression<Func<T, bool>> predicate, params string[] includeProperties)
        {
            var items = await GetAll().ToListAsync();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.AsQueryable().Include(includeProperty).ToList();
                }
            }
            return items.AsQueryable().Where(predicate);
        }

        public async Task<T> FindById(K id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByCondition(predicate).FirstOrDefaultAsync();
        }

        public async Task Remove(T entity)
        {
            DeleteItem(entity);
            await SaveChanges();
        }

        public async Task Remove(K id)
        {
            await Remove(await FindById(id));
            await SaveChanges();
        }

        public async Task RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await SaveChanges();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveChanges();
        }

        private IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        private IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression);
        }

        public void DeleteItem(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> NewFindAll(Expression<Func<T, bool>> predicate, params string[] includeProperties)
        {
            var items = await GetAll().ToListAsync();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.AsQueryable().Include(includeProperty).ToList();
                }
            }
            return items.AsQueryable().Where(predicate);
        }
    }
}
