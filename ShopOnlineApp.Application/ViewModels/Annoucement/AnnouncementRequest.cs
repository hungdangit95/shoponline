using System;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Annoucement
{
    public class AnnouncementRequest:BaseRequest
    {
        public Guid UserId { get; set; }
    }
}
