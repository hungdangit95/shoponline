using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Application.ViewModels.User;
using ShopOnlineApp.Authorization;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.SignalR;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHubContext<OnlineShopHub> _hubContext;
        public UserController(IUserService userService, IAuthorizationService authorizationService, IHubContext<OnlineShopHub> hubContext)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Authentication/NoAuthenication");
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            var model = await _userService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaging(UserRequest request)
        {
            var model = await _userService.GetAllPagingAsync(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (userVm.Id == null)
            {
                var announcement = new AnnouncementViewModel()
                {
                    Content = $"User {userVm.UserName} has been created",
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    Title = "User created",
                    UserId = User.GetUserId(),
                    DateModified = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                };

                await _userService.AddAsync(userVm);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            }
            else
            {
                userVm.DateModified = DateTime.Now;
                await _userService.UpdateAsync(userVm);
            }

            _userService.SaveChanges();

            return new OkObjectResult(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _userService.DeleteAsync(id);

            return new OkObjectResult(id);
        }
    }
}