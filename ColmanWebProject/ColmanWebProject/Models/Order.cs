using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double Price { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime Date { get; set; }
    }
}
