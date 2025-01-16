using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Membership
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MemberPin { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
    }
}
