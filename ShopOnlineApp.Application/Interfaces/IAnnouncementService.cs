using System;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Annoucement;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IAnnouncementService
    {
        Task<PagedResult<AnnouncementViewModel>> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);
        Task<bool> MarkAsRead(Guid userId, string id);
        Task AddAnnoucement(AnnouncementViewModel announcementVm);
    }
}
