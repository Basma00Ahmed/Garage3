using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage3.Models.Entities;
namespace Garage3.Data
{
    public class Garage3Context : DbContext
    {
        public DbSet<Garage3.Models.Entities.Member> Member { get; set; }

        public DbSet<Garage3.Models.Entities.Vehicle> Vehicle { get; set; }
        public DbSet<Garage3.Models.Entities.VehicleType> VehicleType { get; set; }
        public DbSet<Garage3.Models.Entities.Spot> Spot { get; set; }
        public Garage3Context (DbContextOptions<Garage3Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Spot>()
                        .HasMany(v => v.Vehicles)
                        .WithMany(s => s.Spots)
                        .UsingEntity<Parking>(
                            p => p.HasOne(p => p.Vehicle).WithMany(v => v.Parkings),
                            p => p.HasOne(p => p.Spot).WithMany(s => s.Parkings));
        }
        public DbSet<Garage3.Models.Entities.Parking> Parking { get; set; }

    }
}
