using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Slide
{
    public class SlideViewModel:ViewModelBase<Data.Entities.Slide,SlideViewModel>
    {

        public int Id { get; set; }
        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(250)]
        public string Description { set; get; }

        [StringLength(250)]
        [Required]
        public string Image { set; get; }

        [StringLength(250)]
        public string Url { set; get; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }

        public string Content { set; get; }

        [StringLength(25)]
        [Required]
        public string GroupAlias { get; set; }

    }
}
