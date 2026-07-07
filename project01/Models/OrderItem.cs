using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderItemId { get; set; } // system generated

        [Required]
        [Range(1, 999)]
        public int quantity { get; set; } // user input

        // foreign key 
        [Required]
        [ForeignKey("Order")]
        public int orderId { get; set; } // system generated 
        public Order Order { get; set; }                 

        // foreign key 
        [Required]
        [ForeignKey("Product")]
        public int productId { get; set; }               
        public Product Product { get; set; }              


        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal unitPrice { get; set; }    
        
    }

}

