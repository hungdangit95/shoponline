using System;
using System.ComponentModel.DataAnnotations;

namespace ShopOnlineApp.Application.ViewModels.Annoucement
{
    public class AnnouncementUserViewModel
    {
        public int Id { set; get; }

        [StringLength(128)]
        [Required]
        public string AnnouncementId { get; set; }

        public Guid UserId { get; set; }

        public bool? HasRead { get; set; }

    }
}
