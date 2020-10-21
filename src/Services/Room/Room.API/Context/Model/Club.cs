using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Room_Services.API.Context.Model
{
    public class Club 
    {
        public Guid ClubId { get; set; }

        public List<Room> Rooms { get; set; }

        public Club() { }
    }
}