using System.ComponentModel.DataAnnotations;

namespace ShopOnlineApp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

         [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        [Display(Name="Gender")]
        public bool Gender { get; set; }


    }
}
