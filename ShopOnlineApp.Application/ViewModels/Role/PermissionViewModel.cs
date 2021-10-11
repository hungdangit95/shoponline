using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Role
{
    public class PermissionViewModel:ViewModelBase<PermissionViewModel,Permission>
    {
        public int Id { get; set; }


        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        public AppRoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}
