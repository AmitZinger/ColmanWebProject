using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ColmanWebProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
     
        [Required] [Display(Name = "Customer ID")] 
        public int CustomerId { get; set; }
        
        [Required] [Display(Name = "Product ID")]
        public int ProductId { get; set; }
    }
}
