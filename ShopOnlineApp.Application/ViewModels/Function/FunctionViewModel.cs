using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.ViewModels.Function
{
    public class FunctionViewModel:ViewModelBase<FunctionViewModel, Data.Entities.Function>
    {
        public string Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { set; get; }
        [Required]
        [StringLength(250)]
        public string URL { set; get; }
        [StringLength(128)]
        public string ParentId { set; get; }
        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }

        public bool IsUpdated { get; set; }
    }
}
