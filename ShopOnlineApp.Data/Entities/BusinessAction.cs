using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("BusinessAction")]
    public class BusinessAction:DomainEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        
        public string BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public virtual Business Business { get; set; }

        public virtual ICollection<GrantPermission> GrantPermissions { get; set; }

    }
}
