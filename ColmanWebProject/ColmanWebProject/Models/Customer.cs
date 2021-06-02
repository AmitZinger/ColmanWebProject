using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ColmanWebProject.Models
{
    public enum Role {
        Manager,
        Client
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required] 
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Please enter a valid password, between 5 to 10 chars")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please enter a valid name")]
        [StringLength(20,MinimumLength = 2, ErrorMessage = "Please enter a valid name")]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please enter a valid last name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter a valid last name")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }
        public Role Role { get; set; } = Role.Client;
    }
}
