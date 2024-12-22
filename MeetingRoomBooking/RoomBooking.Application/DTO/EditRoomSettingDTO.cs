using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record EditRoomSettingDTO
    {
        public required Guid Id { get; set; }
        public required int MinimumCapacity { get; set; }
        public required int MaximumCapacity { get; set; }
        public required DateTime LastUpdatedAtUTC { get; set; }
    }
}
