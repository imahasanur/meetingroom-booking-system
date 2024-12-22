using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.EventTime
{
    public interface IEventTimeManagementService
    {
        Task<string> EditEventTimeLimitAsync(EditEventTimeDTO eventTimeDTO);
        Task<GetEventTimeDTO> GetTimeLimitAsync();


    }
}
