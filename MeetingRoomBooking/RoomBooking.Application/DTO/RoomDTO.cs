using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record RoomDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required int Capacity { get; set; }
        public required string Details { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }

    }
}
