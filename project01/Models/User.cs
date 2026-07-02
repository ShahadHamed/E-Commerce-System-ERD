using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project01.Models
{
    public class User
    {
        [Primary Key], auto-generated, not null
        public int userId { get; set; } // Sys generate 

        [Required]
        [MaxLength(50)]
        public string userName { get; set; }// User input
                                            
        [Required]
        [MaxLength(256)]
        public string passwordHash { get; set; }// Sys generate

        [Required]
        [MaxLength(150)]
        public string email { get; set; }// User input 
        [Required]
        [MaxLength(100)]
        public string fullName { get; set; }// User input
        [MaxLength(20)]
        public string phoneNumber { get; set; }// User input 
        [MaxLength(300)]
        public string address { get; set; }// User input 
        [Required]

        public DateTime registrationDate { get; set; } //Sys generate 

        public bool isActive { get; set; } = false; // Default value is false, user must verify email to activate account

    }
}
