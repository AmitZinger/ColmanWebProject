using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanWebProject.Models
{
    public class Store
    {
        public int Id { get; set; }

        [Required]
        public double Lontitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        [DataType(DataType.Time)]
        public string OpeningHour { get; set; }

        [DataType(DataType.Time)]
        public string ClosingHour { get; set; }
    }
}
