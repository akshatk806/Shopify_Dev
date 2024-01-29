using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DomainModels
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
