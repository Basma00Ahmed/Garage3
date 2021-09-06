using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
