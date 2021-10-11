using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Feedbacks")]
    public class Feedback : DomainEntity<int>,ISwitchable, IDateTracking
    {

        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(250)]
        public string Email { set; get; }

        [StringLength(500)]
        public string Message { set; get; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}
