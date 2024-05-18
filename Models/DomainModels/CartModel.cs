using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DomainModels
{
    public class CartModel
    {
        [Key]
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public Guid ProductRefId { get; set; }
        public DateTime CartAddedAt { get; set; } = DateTime.Now;
    }
}
