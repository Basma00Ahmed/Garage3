using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int NoOfWheels { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime ArrivalTime { get; set; }
      
        public int VehicleTypeId { get; set; }
        public int MemberId { get; set; }

        public VehicleType VehicleType { get; set; }
        public Member Member { get; set; }

        public ICollection<Spot> Spots { get; set; }

    }
}
