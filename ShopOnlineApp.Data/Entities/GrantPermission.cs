using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("GrantPermission")]
    public class GrantPermission:DomainEntity<int>
    {
        
        [Column(Order = 1)]
        public int BusinessActionId { get; set; }
        [Column(Order = 2)]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        [ForeignKey("BusinessActionId")]
        public virtual BusinessAction BusinessAction { get; set; }
    }
}
