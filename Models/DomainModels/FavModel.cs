using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DomainModels
{
    public class FavModel
    {
        [Key]
        public Guid FavId { get; set; }
        public string UserId { get; set; }
        public Guid ProductRefId { get; set; }
        public DateTime FavAddedAt { get; set; } = DateTime.Now;
    }
}
