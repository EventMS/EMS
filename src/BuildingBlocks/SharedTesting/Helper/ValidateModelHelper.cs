using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Shared.Testing.Helpers
{
    public static class ValidateModelHelper
    {
        /// <summary>
        /// Source: https://stackoverflow.com/questions/2167811/unit-testing-asp-net-dataannotations-validation
        /// Source2: https://github.com/ovation22/DataAnnotationsValidatorRecursive
        /// Validates an object based on the data annotations set on the object's properties and returns the validation result.
        /// </summary>
        /// <param name="model">The object to validate</param>
        /// <returns>The validation result for the object.</returns>
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}