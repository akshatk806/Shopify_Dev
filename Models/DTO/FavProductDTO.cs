namespace Product_Management.Models.DTO
{
    public class FavProductDTO
    {
        public Guid FavId { get; set; }
        public string UserId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductPrice { get; set; }

        public string ProductImageURL { get; set; }

        public DateTime FavAddedAt { get; set; }
    }
}
