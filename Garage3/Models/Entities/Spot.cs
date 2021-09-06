using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Entities
{
    public class Spot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
  
        public int GarageId { get; set; }

        public Garage Garage { get; set; }

        public ICollection<Parking> Parkings { get; set; }
    }
}
