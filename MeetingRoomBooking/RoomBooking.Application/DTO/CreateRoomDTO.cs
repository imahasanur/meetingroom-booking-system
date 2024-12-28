using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record CreateRoomDTO
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required int Capacity { get; set; }
        public required string Details { get; set; }
        public required string CreatedBy { get; set; }
        public required string Color { get; set; }
        public required DateTime CreatedAtUTC { get; set; }
        public int? MaximumCapacity { get; set; }
        public int? MinimumCapacity { get; set; }

    }
}
