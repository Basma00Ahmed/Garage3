using Bogus;
using Garage_3._0.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garage_3._0.Data
{
    public class SeedData
    {
        private static Faker _faker;

        internal static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<AppDbContext>())
            {
                if (await db.Vehicles.AnyAsync()) return;

                _faker = new Faker("sv");

                var types = GetVehicleTypes();
                await db.AddRangeAsync(types);

                var members = GetMembers();
                await db.AddRangeAsync(members);

                await db.SaveChangesAsync();
            }
        }

        private static List<Member> GetMembers()
        {
            var members = new List<Member>();

            for (int i = 0; i < 10; i++)
            {
                var member = new Member
                {
                    SocialSecurityNo = _faker.Name.FullName(),
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    Vehicles = new List<Vehicle>{
                        new Vehicle 
                        { 
                            RegNo = _faker.Vehicle.Vin(), 
                            Make = _faker.Vehicle.Manufacturer(),
                            Model = _faker.Vehicle.Model(),
                            ArrivalTime = DateTime.Now,
                            IsParked = true,
                            MemberId = _faker.Random.Int(1, 10),
                            VehicleTypeId = _faker.Random.Int(1, 5)
                        }
                    }
                };
                members.Add(member);
            }
            return members;
        }

        private static List<VehicleType> GetVehicleTypes()
        {
            return new List<VehicleType>
            {
                new VehicleType { Type = "Car", Size = 1.0 },
                new VehicleType { Type = "Bus", Size = 3.0 },
                new VehicleType { Type = "MotorCycle", Size = 0.33 },
                new VehicleType { Type = "Truck", Size = 4.0 },
                new VehicleType { Type = "Bicycle", Size = 0.25 }
            };
        }

    }
}
