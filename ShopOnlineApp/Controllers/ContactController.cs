using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Models;
using ShopOnlineApp.Services;
using ShopOnlineApp.SignalR;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IFeedbackService _feedbackService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;
        private readonly IHubContext<OnlineShopHub> _hubContext;

        public ContactController(IContactService contactSerivce,
            IViewRenderService viewRenderService,
            IConfiguration configuration,
            IEmailSender emailSender, IFeedbackService feedbackService, IHubContext<OnlineShopHub> hubContext)
        {
            _contactService = contactSerivce;
            _feedbackService = feedbackService;
            _hubContext = hubContext;
            _emailSender = emailSender;
            _configuration = configuration;
            _viewRenderService = viewRenderService;
        }
        [Route("contact.html")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contact = await _contactService.GetById(CommonConstants.DefaultContactId);
            var model = new ContactPageViewModel { Contact = contact };
            return View(model);
        }

        [Route("contact.html")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(ContactPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _feedbackService.Add(model.Feedback);
                _feedbackService.SaveChanges();
                var announcement = new AnnouncementViewModel()
                {
                    Content = $"Bạn có một phản hồi từ {model.Feedback.Name}",
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    Title = "New Order",
                    UserId = User.GetUserId(),
                    Id = Guid.NewGuid().ToString()
                };
                //  await _hubContext.Clients.Client("").SendAsync("ReceiveMessage", announcement);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);

                var content = await _viewRenderService.RenderToStringAsync("Contact/_ContactMail", model.Feedback);
                await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "Have new contact feedback", content);
                ViewData["Success"] = true;
            }

            model.Contact =await _contactService.GetById("default");

            return View("Index", model);
        }
    }
}