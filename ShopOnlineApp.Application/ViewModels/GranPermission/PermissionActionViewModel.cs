using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOnlineApp.Application.ViewModels.GranPermission
{
    public class PermissionActionViewModel
    {
        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public bool IsGranted { get; set; }
    }
}
