using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DomainModels
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string UserId { get; set; }
        public Guid ProductRefId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
