using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Entities
{
    public class Guest : IEntity<Guid>
    {
        public Guid Id { get ; set ; }
        public Guid EventId { get ; set ; }
        public string User { get ; set ; }
        public Event Event { get ; set ; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
    }
}
