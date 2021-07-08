using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ColmanWebProject.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }
        
        [DataType(DataType.Text)]
        public string SubType { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public Image Thumbnail { get; set; }

        public List<Product> Products { get; set; }
    }
}
