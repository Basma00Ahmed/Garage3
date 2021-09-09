using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage_3._0.Models.Entities
{
    public class Spot
    {
        public int Id { get; set; }
        public int ParkingNo { get; set; }
        public double Capacity { get; set; } // 0-1

        // Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
