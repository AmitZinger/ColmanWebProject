using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ColmanWebProject.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid stock price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid stock number")]
        public int Stock { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductsWishList> ProductsWishList { get; set; } = new List<ProductsWishList>();
        public ICollection<ProductsCart> productsCarts { get; set; } = new List<ProductsCart>();
        public ICollection<ProductsOrder> productsOrders { get; set; } = new List<ProductsOrder>();

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
