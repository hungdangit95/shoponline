using System;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.GranPermission
{
    public class GrantPermissionViewModel:ViewModelBase<GrantPermission,GrantPermissionViewModel>
    {
        public int Id { get; set; }
        public int BusinessActionId { get; set; }
        public Guid UserId { get; set; }
    }
}
