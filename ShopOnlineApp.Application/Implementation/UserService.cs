using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.User;
using ShopOnlineApp.Data.EF;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        //chạy thử xem
        private readonly AppDbContext _context;

        public UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor,
            RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<bool> AddAsync(AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                DateModified = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded && userVm.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles);
            }
            return true;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return new AppUserViewModel().Map(await _userManager.Users.AsNoTracking().ToListAsync()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<AppUserViewModel>>> GetAllPagingAsync(UserRequest request)
        {
            var query = _userManager.Users.AsNoTracking().AsParallel();

            if (!string.IsNullOrEmpty(request?.SearchText))
            {
                query = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Where(x => x.FullName.Contains(request.SearchText)
                                         || x.UserName.Contains(request.SearchText)
                                         || x.Email.Contains(request.SearchText));
            }

            int totalRow =  query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Count();

            if (request != null)
                query = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Skip((request.PageIndex) * request.PageSize)
                    .Take(request.PageSize);

            var items = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            });

            var result = new BaseReponse<ModelListResult<AppUserViewModel>>
            {
                Data = new ModelListResult<AppUserViewModel>()
                {
                    Items = items.ToList(),
                    Message = Message.Success,
                    RowCount = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int) QueryStatus.Success
            };
            await Task.CompletedTask;
            return result;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new AppUserViewModel().Map(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }

        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());

            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                await RemoveRolesFromUser(user.Id.ToString(), needRemoveRoles);

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                user.DateModified=DateTime.Now;
                user.DateCreated=DateTime.Now;
                await _userManager.UpdateAsync(user);
            }
        }

        public string GetUserId()
        {
           return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task RemoveRolesFromUser(string userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var roleIds = _roleManager.Roles.AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(3).Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
            List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();
            foreach (var roleId in roleIds)
            {
                userRoles.Add(new IdentityUserRole<Guid> { RoleId = roleId, UserId = user.Id });
            }

            _context.UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync();

        }
    }
}
