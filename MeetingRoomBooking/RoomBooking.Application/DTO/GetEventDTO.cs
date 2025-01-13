using RoomBooking.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record GetEventDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
        public string FontColor { get; set; }
        public required string State { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string Description {get; set;}
        public required string CreatedBy { get; set; }
        public required string Host { get; set; }
        public required Guid RoomId { get; set; }
        public List<Guest> Guests { get; set; }
        public Room Room { get; set; }

    }
}
