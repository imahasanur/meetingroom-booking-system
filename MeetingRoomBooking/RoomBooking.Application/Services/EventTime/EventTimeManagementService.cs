using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Data.Common;
using System.Linq;

namespace RoomBooking.Application.Services.EventTime
{
    public class EventTimeManagementService : IEventTimeManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public EventTimeManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
    }
}