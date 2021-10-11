using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Role
{
    public class AppRoleViewModel:ViewModelBase<AppRoleViewModel,AppRole>
    {
        public Guid? Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
}
