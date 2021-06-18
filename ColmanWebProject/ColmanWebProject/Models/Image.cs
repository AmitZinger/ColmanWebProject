using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ColmanWebProject.Models
{
    public class Image
    {
        public int Id { get; set; }

        [DataType(DataType.ImageUrl)]
         public string Url { get; set; }


        //public byte[] ImageByte { get; set; }

        //[NotMapped]
       // public IFormFile ImageFile { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
