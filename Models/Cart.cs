using System.Collections.Generic;
using System.Linq;

namespace TeamEnigma.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Associate with a user
        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal TotalPrice => CartItems.Sum(item => item.TotalPrice);
    }
}
