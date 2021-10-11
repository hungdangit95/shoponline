using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Feedback;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;
        public FeedBackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllPaging(FeedbackRequest request)
        {
            var model = await _feedbackService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _feedbackService.Delete(id);
            _feedbackService.SaveChanges();
            return new OkObjectResult(id);
        }

    }
}