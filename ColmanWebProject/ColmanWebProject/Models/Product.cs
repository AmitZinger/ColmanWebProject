using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        public Image Image { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public List<Category> Categories { get; set; }

    }
}
