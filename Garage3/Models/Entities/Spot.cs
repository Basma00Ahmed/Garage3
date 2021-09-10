using System.Collections.Generic;

namespace Garage3.Models.Entities
{
    public class Spot
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }

        public int GarageId { get; set; }
        public double Capacity { get; set; }

        public Garage Garage { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
