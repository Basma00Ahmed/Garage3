using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels.Members
{
    public class MembersVehiclesViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Personal ID")]
        public string PersonalNo { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName  { get; set; }
        [Display(Name = "Vehicles Count")]
        public int NoOfVehicles { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

    }
}
