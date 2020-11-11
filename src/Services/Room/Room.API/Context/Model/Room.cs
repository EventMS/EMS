using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using Microsoft.EntityFrameworkCore.Internal;

namespace EMS.Room_Services.API.Context.Model
{
    public class Room : IValidatableObject
    {
        public Guid RoomId { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public List<Booking> Bookings { get; set; }

        public Room() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            if (Bookings != null && Bookings.Count != 0)
            {
                Bookings.Sort((b1, b2) => b2.StartTime.CompareTo(b1.StartTime));
                for (int i = 0; i < Bookings.Count - 1; i++)
                {
                    if (Bookings[i].StartTime < Bookings[i+1].EndTime && Bookings[i+1].StartTime < Bookings[i].EndTime)
                    {
                        yield return new ValidationResult("Overlapping bookings", new []{nameof(Bookings)});
                    }
                }
            }

            yield break;
        }
    }
}
