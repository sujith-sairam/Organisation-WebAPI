using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Models;

namespace Organisation_WebAPI.Data
{
    public class OrganizationContext : DbContext
    {
        public OrganizationContext(DbContextOptions<OrganizationContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Admin> Admins => Set<Admin>(); 
    }
}