using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Domain.Entities
{
    public class Room : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public string Details { get; set; }
        public string CreatedBy { get; set; }
        public Guid ConcurrencyToken { get; set; }
        public int? MinimumCapacity { get; set; }
        public int? MaximumCapacity { get; set; }
        public string QRCode { get; set; }
        public string? RoomImage { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
        public List<Event> Events { get; set; }
       
    }
}
