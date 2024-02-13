using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Product_Management.Models.DomainModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Management.Models.DTO
{
    public class AddProductRequestDTO
    {
        [Required(ErrorMessage = "*Product Name is Required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "*Product Description is Required")]
        [DataType(DataType.MultilineText)]
        public string ProductDesc { get; set; }

        [Required(ErrorMessage = "*Product Price is Required")]
        public int ProductPrice { get; set; }

        public DateTime ProductCreatedAt { get; set; } = DateTime.Now;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public bool IsTrending { get; set; } = false;

        public int CategoryId { get; set; }
        [ValidateNever]
        public string ProductImageURL { get; set; }
        [Required(ErrorMessage = "*Product Image is Required")]
        public IFormFile ImagePath { get; set; }

        // navigation property
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

    }
}
