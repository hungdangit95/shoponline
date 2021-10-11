using System;
using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Annoucement
{
    public class AnnouncementViewModel:ViewModelBase<Announcement, AnnouncementViewModel>
    {
        public string Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { set; get; }

        [StringLength(250)]
        public string Content { set; get; }

        public Guid UserId { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}
