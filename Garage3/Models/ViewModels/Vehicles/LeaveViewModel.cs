using Garage3.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels.Vehicles
{
    public class LeaveViewModel
    {
        public int Id { get; set; }

        [Display (Name = "Reg No")]
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