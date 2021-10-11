using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Business")]
    public class Business : DomainEntity<string>
    {
        public string  Name { get; set; }
        public virtual ICollection<BusinessAction> BusinessActions { get; set; }
    }
}
