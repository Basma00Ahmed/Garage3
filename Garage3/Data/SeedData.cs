using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Models.Entities;

namespace Garage3.Data
{
    public class SeedData
    {
        private static Faker fake;

        internal static async Task InitAsync(IServiceProvider services)
        {
            using(var db = services.GetRequiredService<Garage3Context>())
            {
                if (await db.Member.AnyAsync()) return;

                fake = new Faker("sv");

                var Members = GetMembers();
                await db.AddRangeAsync(Members);

                await db.SaveChangesAsync();
            }
        }

  
        private static List<Member> GetMembers()
        {
            var Members = new List<Member>();

            for (int i = 0; i < 200; i++)
            {
                var fName = fake.Name.FirstName();
                var lName = fake.Name.LastName();
                var Member = new Member
                {
                    FirstName = fName,
                    LastName = lName,
                    Vehicles = new List<Vehicle>{
                                     new Vehicle
                                    {
                                        RegNo = fake.Vehicle.Vin(),
                                        Make = fake.Vehicle.Manufacturer(),
                                        Model = fake.Vehicle.Model(),
                                        ArrivalTime=DateTime.Now,
                                        NoOfWheels=4,
                                        IsCheckedOut=false,
                                        VehicleTypeId = 1
                                     }
                                                }
                };
                Members.Add(Member);
            }

            return Members;
        }
    }
}
