using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ShopOnlineApp.Data.Entities
{
    [Table("AppUserRoles")]
    public class AppUserRole:IdentityUserRole<Guid>
    {
    }
}
