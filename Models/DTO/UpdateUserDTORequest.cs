using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class UpdateUserDTORequest
    {
        [Key]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
