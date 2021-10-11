using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class FeedbackRepository : EFRepository<Feedback, int>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}
