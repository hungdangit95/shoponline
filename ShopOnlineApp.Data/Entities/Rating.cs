using System;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;
namespace ShopOnlineApp.Data.Entities
{
    public class Rating : DomainEntity<int>,IDateTracking
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Value { get; set; }
        public string NickName { get; set; }
        public string Summary { get; set; }
        public string Review { get; set; }
        public DateTime DateCreated { get; set;}
        public DateTime DateModified { get;set;}
    }
}
