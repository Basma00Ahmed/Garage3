using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Garage3.Models.ViewModels.Vehicles
{

    public class RegistrationViewModel
    {
        public int Id { get; set; }


        [RegularExpression(@"(19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])-\d{4}$", ErrorMessage = "Invalid format ex: 19880414-1234.")]
        [Remote(action: "VerifyPersonalNo", controller: "Vehicles")]
        public string PersonalNo { get; set; }

        [RegularExpression(@"([a-zA-ZäöåÄÖÅ)]{3})([\s]{1})([0-9]{3})$", ErrorMessage = "Invalid format ex: ABC 123.")]
        [Required]
        [MaxLength(7)]
        // [Remote(action: "VerifyRegNo", controller: "ParkedVehicles")] släckt så länge
        [Display(Name = "Registration No")]
        public string RegNo { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z \s)]{3,}$", ErrorMessage = "This is not a valid Make")]
        public string Make { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"[a-zA-Z0-9) \s]{2,}$", ErrorMessage = "This is not a valid Model")]
        public string Model { get; set; }

        [RegularExpression(@"[a-zA-ZäöåÄÖÅ)]{3,}$", ErrorMessage = "This is not a valid color")]
        [MaxLength(10)]
        public string Color { get; set; }

        [Range(0, 12, ErrorMessage = "Number of wheels must be between 0 and 12")]
        [Display(Name = "Number of wheels")]
        public int NoOfWheels { get; set; }

        public bool IsCheckedOut { get; set; }


        [Display(Name = "Arrival time")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Display(Name = "Vehicle type")]
        public int VehicleTypeId { get; set; }

        public int MemberId { get; set; }

        public VehicleType VehicleType { get; set; }
        public Member Member { get; set; }

        public ICollection<Spot> Spots { get; set; }
    }
}
