using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
        public int Stock { get; set; }

        public string Description { get; set; }
    }
}
