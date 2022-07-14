using CarSalesPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSalesPortal.AppDbControl
{
    public class CarSalesPortalDbContext:IdentityDbContext<IdentityUser>
    {
        public CarSalesPortalDbContext(DbContextOptions<CarSalesPortalDbContext> options):
            base(options)
        { 

        }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Car> Cars{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
