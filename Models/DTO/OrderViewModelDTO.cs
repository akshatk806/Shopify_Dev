namespace Product_Management.Models.DTO
{
    public class OrderViewModelDTO
    {
        public Guid OrderId { get; set; }
        public int ProductPrice { get; set; }
        public string TransactionId { get; set; }
        public int ProductQuantity { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
    }
}
