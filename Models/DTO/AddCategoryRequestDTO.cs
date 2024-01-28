using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DTO
{
    public class AddCategoryRequestDTO
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
