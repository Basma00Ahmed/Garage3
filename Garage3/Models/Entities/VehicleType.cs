using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        [Required]
        [Remote(action: "VerifyVehicleType", controller: "VehicleType")]
        public string Type { get; set; }
        [Required]
        public double Size { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}