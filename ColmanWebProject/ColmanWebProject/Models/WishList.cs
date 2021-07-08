using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class WishList
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
