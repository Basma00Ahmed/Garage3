using System;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels.Vehicles
{
    public class LeaveViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Reg No")]
        public string RegNo { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Owner")]
        public string MemberFullName { get; set; }
    }
}