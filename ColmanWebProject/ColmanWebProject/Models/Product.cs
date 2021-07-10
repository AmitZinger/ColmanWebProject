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
        [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<WishList> WishLists { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
