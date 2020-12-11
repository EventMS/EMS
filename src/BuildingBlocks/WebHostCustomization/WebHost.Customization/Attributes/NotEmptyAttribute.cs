using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.TemplateWebHost.Customization.Attributes
{
    /// <summary>
    /// Checks whether current date is in the future
    /// Based on: https://stackoverflow.com/questions/14945536/mvc-validate-date-time-is-at-least-1-minute-in-the-future
    /// </summary>
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (DateTime)value > DateTime.Now;
        }
    }

    /// <summary>
    /// Checks whether current Guid is empty
    /// Based on: https://andrewlock.net/creating-an-empty-guid-validation-attribute/
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotEmpty doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}
