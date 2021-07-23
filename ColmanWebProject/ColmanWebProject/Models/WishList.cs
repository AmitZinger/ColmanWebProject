using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class WishList
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public ICollection<ProductsWishList> ProductsWishList { get; set; } = new List<ProductsWishList>();
    }
}
