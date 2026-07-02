using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Category
    {
        public int categoryId { get; set; } // Sys generate

        [Required]
        [MaxLength(100)]
        public string categoryName { get; set; } // User input
        [MaxLength(500)]
        public string description { get; set; }// User input
        [MaxLength(300)]
        public string imageUrl { get; set; }// User input 

        // Navigation Property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
