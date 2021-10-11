using System;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("ProductQuantities")]
    public class ProductQuantity : DomainEntity<int>
    {
        [Column(Order = 1)]
        public int ProductId { get; set; }
        [Column(Order = 2)]
        public int SizeId { get; set; }
        [Column(Order = 3)]
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }
        //public DateTime DateCreated { get; set; }  
        //public DateTime DateModified { get; set; }
    }
}
