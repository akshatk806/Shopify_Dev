using Product_Management.Models.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Product_Management.Models.DTO
{
    public class CartProductDTO
    {
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductPrice { get; set; }

        public string ProductImageURL { get; set; }

        public DateTime CartAddedAt { get; set; }
    }

}
