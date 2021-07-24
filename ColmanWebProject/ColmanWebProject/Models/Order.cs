using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ColmanWebProject.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; } = 0;

        [Required]
        [Display(Name = "City")]
        public string ShippingAddressCity { get; set; }
        [Required]
        [Display(Name = "Street")]
        public string ShippingAddressStreet { get; set; }
        [Required]
        [Display(Name = "Home number")]
        public string ShippingAddressHomeNum { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } 

        [Required]
        public ICollection<ProductsOrder> productsOrders { get; set; } = new List<ProductsOrder>();

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
