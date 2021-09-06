using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Size { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
