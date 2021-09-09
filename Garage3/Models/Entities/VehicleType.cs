using System.Collections.Generic;

namespace Garage_3._0.Models.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Size { get; set; } //0.33-3

        // Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
