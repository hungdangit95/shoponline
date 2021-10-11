using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Contact
{
    public class ContactViewModel:ViewModelBase<Data.Entities.Contact,ContactViewModel>
    {
        public string Id { set; get; }
        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(50)]
        public string Phone { set; get; }

        [StringLength(250)]
        public string Email { set; get; }

        [StringLength(250)]
        public string Website { set; get; }

        [StringLength(250)]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public Status Status { set; get; }
    }
}
