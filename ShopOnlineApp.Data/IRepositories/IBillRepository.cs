using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Data.IRepositories
{
    public interface IBillRepository : IRepository<Bill, int>
    {
        Task<Bill> AddAsync(Bill entity);
    }
}
