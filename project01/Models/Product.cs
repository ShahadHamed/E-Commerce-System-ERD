using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; } // Sys generate 

        [Required]
        [MaxLength(150)]
        public string productName { get; set; } // User input 

        [MaxLength(1000)]
        public string description { get; set; } // User input 

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; } // User input 

        [Required]
        [Range(0, int.MaxValue)]
        public int stockQuantity { get; set; } // User input 

        [ForeignKey(nameof(Category))]
        public int categoryId { get; set; } // User input 

        [MaxLength(300)]
        public string imageUrl { get; set; }// User input
        [Required]
        public DateTime createdAt { get; set; } // Sys generate

        public bool isAvailable { get; set; } = true; // Default value is true, user can set to false to hide product from listing
    }
}
