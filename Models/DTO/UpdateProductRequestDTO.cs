using Product_Management.Models.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [ValidateNever]
        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
