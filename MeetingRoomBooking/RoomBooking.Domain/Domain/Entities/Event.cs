﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Domain.Entities
{
    public class Event:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }    
        public string Host { get; set; }
        public Room Room { get; set; }
        public Guid RoomId { get; set; }
        public List<Guest> Guests { get; set; }
        public DateTime CreatedAtUTC { get ; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
    }
}
