using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    public class BlogComment : DomainEntity<int>, IDateTracking
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int BlogId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }

    }
}
