using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record CreateEventDTO
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public string Host { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CreatedAtUTC { get; set; }

    }
}
