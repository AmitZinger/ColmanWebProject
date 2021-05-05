using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Type { get; set; }
        public string Description { get; set; }

        public int ImageId { get; set; }
        public Image Thunbnail { get; set; }
    }
}
