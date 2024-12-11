using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Domain.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastUpdatedAtUtc { get; set; }
    }
}
