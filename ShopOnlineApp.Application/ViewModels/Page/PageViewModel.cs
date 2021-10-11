using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Page
{
    public class PageViewModel:ViewModelBase<Data.Entities.Page,PageViewModel>
    {
        public int Id { set; get; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }
        public Status Status { set; get; }
    }
}
