using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}
