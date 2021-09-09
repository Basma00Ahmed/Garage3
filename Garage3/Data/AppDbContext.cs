using Garage_3._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Garage_3._0.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Member> Members { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    }
}
