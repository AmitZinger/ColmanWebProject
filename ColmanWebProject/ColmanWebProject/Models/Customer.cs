using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ColmanWebProject.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required] 
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required] 
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
