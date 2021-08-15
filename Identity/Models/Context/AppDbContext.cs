using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models.Context
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) 
            :base(dbContextOptions)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
