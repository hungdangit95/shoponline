using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.Interfaces;

namespace ShopOnlineApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public string FullName { get; set; }
        public DateTime? BirthDay { set; get; }
        public decimal Balance { get; set; }
        public string Avatar { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
    }
}
