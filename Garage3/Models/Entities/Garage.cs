using System.Collections.Generic;

namespace Garage3.Models.Entities
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        public ICollection<Spot> Spots { get; set; }
    }
}
