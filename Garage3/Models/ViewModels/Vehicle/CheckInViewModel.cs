using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage_3._0.Models.ViewModels.Vehicle
{
    public class CheckInViewModel
    {
        public string RegNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int VehicleTypeId { get; set; }
    }
}
