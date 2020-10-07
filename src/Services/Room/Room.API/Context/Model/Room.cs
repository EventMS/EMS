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
            //Some logic to no overlap in intervals in bookings. 
            //SortedList<DateTime, Booking> -> Iteraate and compare EndTime of current to startTime of next. 
            //Prettyfy this
            /*
            SortedList<DateTime, Booking> sortedBookings = new SortedList<DateTime, Booking>();
            foreach (var booking in Bookings)
            {
                sortedBookings.Add(booking.EndTime, booking);
            }

            for (int i = 0; i < sortedBookings.Count -1; i++)
            {
                var sortedBookingEnd = sortedBookings[sortedBookings.Keys[i]];
                var sortedBookingStart = sortedBookings[sortedBookings.Keys[i]];
                if (sortedBookingEnd.EndTime > sortedBookingStart.StartTime)
                {
                    yield return new ValidationResult("Intervals overlap",
                        new[] { nameof(Bookings)});
                }
            }
            */

            yield break;
        }
    }
}
