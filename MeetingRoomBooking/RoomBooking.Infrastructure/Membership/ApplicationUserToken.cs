﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure.Membership
{
    public class ApplicationUserToken
           : IdentityUserToken<Guid>
    {

    }
}
