using System.ComponentModel.DataAnnotations;

namespace ShopOnlineApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

    }
}
