using Product_Management.Validation;
using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name should contains Minimum 3 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number Required!")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^[0-9\-\+]+$", ErrorMessage = "Not a valid phone number")]
        //[PhoneNumber(ErrorMessage = "Not a valid phone number")]
        [RegularExpression(@"^(?![0-5])\d{10}$", ErrorMessage = "Not a valid phone number")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid phone number(Should contains exactly 10 digits)")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(3, ErrorMessage = "Address should contains Minimum 3 characters")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[CustomPassword(ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be between 8-16 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,16}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be between 8-16 characters.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
