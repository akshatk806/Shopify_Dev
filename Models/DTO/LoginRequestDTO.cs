using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class LoginRequestDTO
    {
        [EmailAddress]  
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }    

        public bool IsActive { get; set; } = true;
    }
}
