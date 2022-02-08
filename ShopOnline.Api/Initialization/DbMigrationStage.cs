using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Api.Initialization
{
    public class DbMigrationStage : IStage
    {
        private readonly AppDbContext _dbContext;

        public DbMigrationStage(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Order => 1;
        public async Task ExecuteAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
