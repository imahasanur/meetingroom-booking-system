using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record EditUserDTO
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public bool IsLoggedIn { get; set; }

    }
}
