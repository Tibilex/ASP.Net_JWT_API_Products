using ASP.Net_JWT_API_Products.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ASP.Net_JWT_API_Products.Data
{
    public class DataBaseContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataBaseContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
