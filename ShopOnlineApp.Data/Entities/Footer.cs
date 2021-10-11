using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Footers")]
    public class Footer : DomainEntity<string>
    {

        [Required]
        public string Content { set; get; }
    }
}
