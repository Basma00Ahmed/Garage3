using System;

namespace Garage3.Models.ViewModels.Vehicles
{
    public class RegistrationViewModel
    {
        public string MemberSocialSecurityNo { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string RegNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int VehicleTypeId { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
