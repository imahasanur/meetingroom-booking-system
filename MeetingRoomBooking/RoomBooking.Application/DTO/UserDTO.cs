using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record UserDTO
    {
        public required Guid Id { get; set; }
        public required string UserEmail {get; set;}
        public required bool IsLoggedIn { get; set; }
    }
}
