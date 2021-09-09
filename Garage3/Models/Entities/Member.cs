using System.Collections.Generic;

namespace Garage_3._0.Models.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string SocialSecurityNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
