using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.Common.Validations
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return value is bool b && b ? ValidationResult.Success : null;
        }
    }
}