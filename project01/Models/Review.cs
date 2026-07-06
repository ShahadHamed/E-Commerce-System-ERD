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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reviewId { get; set; }                 // system generated

        [Required]
        [Range(1, 5)]
        public int rating { get; set; }                   // user input 

        [MaxLength(1000)]
        public string comment { get; set; }               // user input

        [Required]
        public DateTime reviewDate { get; set; }           // system generated 




        // foreign key 
        [Required]
        [ForeignKey("user")]
        public int userId { get; set; }                   
        public User user { get; set; }                   



        // foreign key — every review is about exactly one product
        [Required]
        [ForeignKey("product")]
        public int productId { get; set; }               

    }
}
