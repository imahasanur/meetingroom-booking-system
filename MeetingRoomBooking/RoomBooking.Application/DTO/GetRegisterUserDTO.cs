using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.DTO
{
    public record GetRegisterUserDTO
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }
        public required string Deaprtment { get; set; }
        public required string MemberPin { get; set; }
        public required DateTime CreatedAtUTC { get; set; }
    }
}
