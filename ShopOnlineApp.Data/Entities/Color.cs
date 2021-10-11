using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}
