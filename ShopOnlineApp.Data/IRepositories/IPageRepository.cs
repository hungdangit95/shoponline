using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Data.IRepositories
{
    public interface IPageRepository : IRepository<Page, int>
    {
    }
}
