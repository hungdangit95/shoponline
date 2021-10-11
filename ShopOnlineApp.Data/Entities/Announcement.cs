using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Announcements")]
    public class Announcement  : DomainEntity<string>,ISwitchable,IDateTracking
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }

        [Required]
        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        public Guid UserId { set; get; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<AnnouncementUser> AnnouncementUsers { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }



    }
}
