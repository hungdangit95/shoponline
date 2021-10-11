using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class BillRepository : EFRepository<Bill, int>, IBillRepository
    {
        private readonly AppDbContext _context;

        public BillRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Bill> AddAsync(Bill entity)
        {
            await _context.Bills.AddAsync(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).GetDatabaseValues();
            return entity;
        }
    }
}
