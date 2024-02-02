using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
