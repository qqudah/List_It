using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TeamEnigma.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        
        public Status Status { get; set; } // Available, Sold

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string ContactNumber { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid image URL format.")]
        public string ImageUrl { get; set; }

        [Required]
        public Category Category { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        //navigation
        public string UserId { get; set; } // Foreign Key for the seller
        public virtual IdentityUser? User { get; set; } // Navigation Property
    }
}
