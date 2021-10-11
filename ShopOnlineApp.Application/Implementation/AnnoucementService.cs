using System;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        #region Private property
        private readonly IRepository<Announcement, string> _announcementRepository;
        private readonly IRepository<AnnouncementUser, int> _announcementUserRepository;
        #endregion

        #region Constructor
        public AnnouncementService(IRepository<Announcement, string> announcementRepository,
            IRepository<AnnouncementUser, int> announcementUserRepository,
            IUnitOfWork unitOfWork)
        {
            _announcementUserRepository = announcementUserRepository;
            _announcementRepository = announcementRepository;
        }
        #endregion

        #region Public method
        public async Task<PagedResult<AnnouncementViewModel>> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in await _announcementRepository.FindAll()
                        join y in await _announcementUserRepository.FindAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annoucementUser in xy.DefaultIfEmpty()
                        where annoucementUser.HasRead == false && (annoucementUser.UserId == null || annoucementUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var model = new AnnouncementViewModel().Map(query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize)).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }
        public async Task<bool> MarkAsRead(Guid userId, string id)
        {
            bool result = false;
            var announEntities = await _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                                && x.UserId == userId);
            if (announEntities == null)
            {
                await _announcementUserRepository.Add(new AnnouncementUser
                {
                    AnnouncementId = id,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announEntities.HasRead == false)
                {
                    announEntities.HasRead = true;
                    await _announcementUserRepository.SaveChanges();
                    result = true;
                }

            }
            return result;
        }
        public async Task AddAnnoucement(AnnouncementViewModel announcementVm)
        {
            var annoucement = new AnnouncementViewModel().Map(announcementVm);
            await _announcementRepository.Add(annoucement);
            await _announcementUserRepository.Add(new AnnouncementUser
            {
                AnnouncementId = annoucement.Id,
                HasRead = false,
                UserId = annoucement.UserId
            });
            await _announcementRepository.SaveChanges();
            await _announcementUserRepository.SaveChanges();
        }
        #endregion
    }
}
