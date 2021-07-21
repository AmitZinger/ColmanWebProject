using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class ProductsCart
    {
        [Column(Order = 0), ForeignKey("Product")]
        public int ProductId { get; set; }
        [Column(Order = 1), ForeignKey("Cart")]
        public int CartId { get; set; }

        public int Quantity { get; set; } = 1;

        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
