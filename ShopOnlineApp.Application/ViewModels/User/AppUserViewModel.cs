using System;
using System.Collections.Generic;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.User
{
    public class AppUserViewModel:ViewModelBase<AppUserViewModel,AppUser>
    {
        public AppUserViewModel()
        {
            Roles = new List<string>();
        }
        public Guid? Id { set; get; }
        public string FullName { set; get; }
        public string BirthDay { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string Address { get; set; }
        public string PhoneNumber { set; get; }
        public string Avatar { get; set; }
        public Status Status { get; set; }
        public DateTime DateModified { set; get; }
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }

        public List<string> Roles { get; set; }
    }
}
