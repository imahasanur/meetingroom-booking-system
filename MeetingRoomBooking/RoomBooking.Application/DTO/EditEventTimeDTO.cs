using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record EditEventTimeDTO
    {
        public required Guid Id { get; set; }
        public required int MinimumTime { get; set; }
        public required int MaximumTime { get; set; }
        public required string UpdatedBy { get; set; }
        public required DateTime LastUpdatedAtUTC { get; set; }
    }
}
