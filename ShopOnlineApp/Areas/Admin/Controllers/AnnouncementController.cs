using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Extensions;
using System.Threading.Tasks;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPaging(int page, int pageSize)
        {
            var model = await _announcementService.GetAllUnReadPaging(User.GetUserId(), page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var result =await _announcementService.MarkAsRead(User.GetUserId(), id);
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnoucement(AnnouncementViewModel announcementViewModel)
        {
            if (announcementViewModel != null)
            {
               await _announcementService.AddAnnoucement(announcementViewModel);
            }

            return new  OkObjectResult("");
        }
    }
}