using System.ComponentModel.DataAnnotations;

namespace TeamEnigma.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public decimal TotalPrice => Quantity * Item.Price;
    }
}
