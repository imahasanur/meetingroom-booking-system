using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Domain.Entities
{
    public class UploadedUser : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
    }
}
