using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ensek.Models
{
    public class ApplicationDbContext:DbContext
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public ApplicationDbContext()
           : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}