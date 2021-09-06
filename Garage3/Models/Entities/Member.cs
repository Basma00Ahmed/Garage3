using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string PersonalNo { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

    }
}
