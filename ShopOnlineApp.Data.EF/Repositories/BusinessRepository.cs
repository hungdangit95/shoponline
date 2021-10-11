using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class BusinessRepository : EFRepository<Business, string>, IBusinessRepository
    {
        public BusinessRepository(AppDbContext context) : base(context)
        {
        }
    }
}
