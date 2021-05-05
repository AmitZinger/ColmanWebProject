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

        public string FullName { get; set; }

        public string Phone { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
