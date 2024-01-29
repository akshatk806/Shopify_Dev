using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class AddProductRequestDTO
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string ProductDesc { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        public DateTime ProductCreatedAt { get; set; } = DateTime.Now;
    }
}
