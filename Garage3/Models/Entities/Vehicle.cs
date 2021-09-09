using System;
using System.Collections.Generic;

namespace Garage_3._0.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsParked { get; set; } // It could be either parked = true, checkout = false or left = deleted from the db.

        // FK
        public int VehicleTypeId { get; set; }
        public int MemberId { get; set; }

        // Navigation Property
        public VehicleType VehicleType { get; set; }
        public Member Member { get; set; }
        public ICollection<Spot> Spots { get; set; }
    }
}
