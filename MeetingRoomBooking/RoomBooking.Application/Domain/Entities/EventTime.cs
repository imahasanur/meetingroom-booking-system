using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Entities
{
    public class EventTime : IEntity<Guid>
    {
        public Guid Id { get ; set ; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
    }
}
