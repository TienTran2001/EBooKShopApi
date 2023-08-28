using System.ComponentModel.DataAnnotations;

namespace EBooKShopApi.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 200 characters")]
        public string HashedPassword { get; set; } = "";  
    }
}

