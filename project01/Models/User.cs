using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class User
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }                  // system generated

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }              // user input

        [Required]
        [MaxLength(150)]
        public string email { get; set; }                 // user input

        [Required]
        [MaxLength(256)]
        public string passwordHash { get; set; }          // system generated 

        [Required]
        [MaxLength(100)]
        public string fullName { get; set; }              // user input

        [MaxLength(20)]
        public string phoneNumber { get; set; }           // user input

        [MaxLength(300)]
        public string? address { get; set; }               // user input

        public DateTime? registrationDate { get; set; }    // system generated 

        public bool isActive { get; set; } = true;        // default value


        // reverse navigation 
        public List<Review> Reviews { get; set; }
        public List<Order> Orders { get; set; }
    }
}
