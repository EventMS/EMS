using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Room_Services.API.Context.Model
{
    public class Booking : IValidatableObject
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid RoomId { get; set; }
        public string Name { get; set; }

        public Guid EventId { get; set; }
        
        public Booking() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            if (StartTime > EndTime)
            {
                yield return new ValidationResult("Startime must be later than endtime",
                    new[] { nameof(StartTime), nameof(EndTime) });
            }
            yield break;
        }
    }
}