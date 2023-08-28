using System.ComponentModel.DataAnnotations;

namespace EBooKShopApi.ViewModels
{
    public class RegisterViewModel
    {
        
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 200 characters")]
        public string HashedPassword { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters")]
        public string? FullName { get; set; }
    }
}
