using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Management.Models.DomainModels
{
    public class Product
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

        public int CategoryId { get; set; }

        // navigation property
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        // image url
        public string ImageURL { get; set; } = "";
    }
}
