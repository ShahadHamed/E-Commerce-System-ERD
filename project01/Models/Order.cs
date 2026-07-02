using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class Order
    {
        public int orderId { get; set; } // Sys generate 
        [ForeignKey(nameof(User))]
        public int userId { get; set; } // User input 

        [Required]
        public DateTime orderDate { get; set; } // Sys generate 

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal totalAmount { get; set; } // Calculated based on order items

        [Required]
        [MaxLength(30)]
        public string status { get; set; } // User input (e.g., "Pending", "Shipped", "Delivered")

        [Required]
        [MaxLength(300)]
        public string shippingAddress { get; set; } // User input 

        [Required]
        [MaxLength(50)]
        public string paymentMethod { get; set; } // User input 

        // Navigation Properties

    }
}
