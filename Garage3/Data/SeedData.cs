using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Models.Entities;
using System.Text;

namespace Garage3.Data
{
    public class SeedData
    {
        private static Faker fake;
        private static DateTime start = new DateTime(1940, 1, 1);
        private static DateTime end = new DateTime(2003, 1, 1);

        internal static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<Garage3Context>())
            {
                if (await db.Vehicle.AnyAsync()) return;

                fake = new Faker("sv");

                var types = GetVehicleTypes();
                await db.AddRangeAsync(types);

                var members = GetMembers();
                await db.AddRangeAsync(members);
                await db.SaveChangesAsync();

                var vehicles = GetVehicles();
                await db.AddRangeAsync(vehicles);



                //           var enrollments = GetEnrollments(vehicles, );
                //           await db.AddRangeAsync(enrollments);

                await db.SaveChangesAsync();
            }
        }

        /*      private static List<Enrollment> GetEnrollments(List<Student> students, List<Course> courses)
              {
                  var enrollments = new List<Enrollment>();
                  foreach (var student in students)
                  {
                      foreach (var course in courses)
                      {
                          if(fake.Random.Int(0,5) == 0)
                          {
                              var enrollment = new Enrollment
                              {
                                  Course = course,
                                  Student = student,
                                  Grade = fake.Random.Int(1, 5)
                              };
                              enrollments.Add(enrollment);
                          }
                      }
                  }
                  return enrollments;
              }
        */
        private static List<Member> GetMembers()
        {
            var members = new List<Member>();

            for (int i = 0; i < 100; i++)
            {
                var fName = fake.Name.FirstName();
                var lName = fake.Name.LastName();
                string fourDigit = new Random().Next(1000, 9999).ToString("D4");
                string birthDay = String.Format("{0:yyyyMMdd}", GetBirthDay());

                var member = new Member
                {
                    FirstName = fName,
                    LastName = lName,
                    PersonalNo = birthDay + "-" + fourDigit
                };

                members.Add(member);
            }

            return members;
        }

        private static List<Vehicle> GetVehicles()
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < 10; i++)
            {
                var regNo = GetRegNo();
                var model = fake.Vehicle.Model();


                var vehicle = new Vehicle
                {
                    RegNo = regNo,
                    Model = model,
                    NoOfWheels = fake.Random.Int(0, 5),
                    IsCheckedOut = false,
                    ArrivalTime = DateTime.Now,
                    MemberId = fake.Random.Int(1, 5),
                    VehicleTypeId = fake.Random.Int(1, 5)
                };
                vehicles.Add(vehicle);
            }

            return vehicles;
        }

        private static List<VehicleType> GetVehicleTypes()
        {
            return new List<VehicleType>
            {
                new VehicleType {Type="Car", Size=1.0},
                new VehicleType {Type="Bus", Size=3.0 },
                new VehicleType {Type="MotorCycle", Size=0.33 },
                new VehicleType {Type="Truck", Size=4.0 },
                new VehicleType {Type="Bicycle", Size=0.25 }
            };
        }

        private static DateTime GetBirthDay()
        {
            int range = (end - start).Days;
            Random random = new Random();
            return start.AddDays(random.Next(range));
        }

        private static string GetRegNo()
        {
            Random random = new Random();
            var builder_char = new StringBuilder(3);
            var builder_int = new StringBuilder(3);
            for (int i = 0; i < 3; i++)
            {
                var c = (char)random.Next(65, 90);
                builder_char.Append(c);
            }
            for (int i = 0; i < 3; i++)
            {
                var x = random.Next(0, 9);
                builder_int.Append(x);
            }
            return builder_char.ToString() + " " + builder_int.ToString();
        }

    }
}