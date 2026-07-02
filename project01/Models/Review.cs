using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Review
    {
        [Key]
        public int reviewId { get; set; } // Sys generate 
        [ForeignKey(nameof(User))]
        public int userId { get; set; } // User input 
        [ForeignKey(nameof(Product))]

        public int productId { get; set; } // User input

        [Required]
        [Range(1, 5)]
        public int rating { get; set; } // User input (e.g., 1-5)

        [MaxLength(1000)]
        public string comment { get; set; } // User input 
        [Required]
        public DateTime reviewDate { get; set; } // Sys generate

        // Navigation Properties
        public User User { get; set; }

        public Product Product { get; set; }
    }
}
