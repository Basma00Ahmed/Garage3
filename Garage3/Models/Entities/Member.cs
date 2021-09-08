using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Garage3.Models.Entities
{
    public class Member
    {
        public int Id { get; set; }
        [RegularExpression(@"(19|20)\d\d(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])-\d{4}$", ErrorMessage = "Invalid format ex: 19880414-1234.")]
        [Remote(action: "VerifyPersonalNo", controller: "Members")]
        public string PersonalNo { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

    }
}
