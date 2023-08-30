using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portfolio_Eastelson.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Mission> Missions { get; set; }

        // Ajoutez d'autres DbSets pour les autres tables

        public ApplicationDbContext() : base("Eastelson")
        {
        }
    }
}