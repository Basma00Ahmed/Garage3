using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Models.Entities;

namespace Garage3.Validation
{
    public class CheckNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "First name cannot be the same as last name";

            if (value is string input)
            {
                var model = (Member)validationContext.ObjectInstance;
                if (model.FirstName != input)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(errorMessage);
            }
            return new ValidationResult(errorMessage);
        }
    }
}