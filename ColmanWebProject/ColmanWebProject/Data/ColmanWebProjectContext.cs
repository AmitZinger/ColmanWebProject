using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Models;

namespace ColmanWebProject.Data
{
    public class ColmanWebProjectContext : DbContext
    {
        public ColmanWebProjectContext (DbContextOptions<ColmanWebProjectContext> options)
            : base(options)
        {
        }

        public DbSet<ColmanWebProject.Models.Product> Product { get; set; }

        public DbSet<ColmanWebProject.Models.Category> Category { get; set; }
    }
}
