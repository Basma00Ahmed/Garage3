using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        public int SpotId { get; set; }
        public int VehicleId { get; set; }
        public Spot Spot { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
