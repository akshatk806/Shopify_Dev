using System.ComponentModel;

namespace Product_Management.Models.DTO
{
    public class ProductCategoryDTO
    {
        public Guid ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string ProductDesc { get; set; } // Product description

        public int ProductPrice { get; set; }

        public DateTime ProductCreatedAt { get; set; }
    }
}
