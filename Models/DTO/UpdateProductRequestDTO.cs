using Product_Management.Models.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class UpdateProductRequestDTO
    {
        [Key]
        public Guid ProductId { get; set; }


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
