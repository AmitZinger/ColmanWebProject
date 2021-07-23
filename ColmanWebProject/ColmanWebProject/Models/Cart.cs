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

        public Customer Customer { get; set; }

        public ICollection<ProductsCart> productsCarts { get; set; } = new List<ProductsCart>();
    }
}
