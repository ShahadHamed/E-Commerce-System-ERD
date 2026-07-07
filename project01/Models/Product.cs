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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productId { get; set; }  // system generated

        [Required]
        [MaxLength(150)]
        public string productName { get; set; } // user input

        [MaxLength(1000)]
        public string description { get; set; } // user input

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public double price { get; set; } // user input

        [Required]
        [Range(0, int.MaxValue)]
        public int stockQuantity { get; set; } = 0;  // default value 

        [MaxLength(300)]
        public string imageUrl { get; set; } // user input

        [Required]
        public DateTime createdAt { get; set; } // system generated 

        public bool isAvailable { get; set; } = true; // default value



        // foreign key 
        [Required]
        [ForeignKey("category")]
        public int categoryId { get; set; }               
        public Category category { get; set; }            


        // reverse navigation 
        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    }
}
