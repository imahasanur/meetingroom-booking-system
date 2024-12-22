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

        public async Task<GetEventTimeDTO> GetTimeLimitAsync()
        {
            var eventTimeEntities = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
            var eventTimeEntity = eventTimeEntities[0];
            var eventDTO = new GetEventTimeDTO { Id = eventTimeEntity.Id, MaximumTime = eventTimeEntity.MaximumTime, MinimumTime = eventTimeEntity.MinimumTime, };
            return eventDTO;
        }
        public async Task<string> EditEventTimeLimitAsync(EditEventTimeDTO eventTimeDTO)
        {
            var response = string.Empty;
            var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitByIdAsync(eventTimeDTO.Id);
            if (eventTimeEntity?.CreatedBy != null) 
            {
                eventTimeEntity.UpdatedBy = eventTimeDTO.UpdatedBy;
                eventTimeEntity.LastUpdatedAtUTC = eventTimeDTO.LastUpdatedAtUTC;
                eventTimeEntity.MinimumTime = eventTimeDTO.MinimumTime;
                eventTimeEntity.MaximumTime = eventTimeDTO.MaximumTime;

            }
            else
            {
                response = "not found";

                return response;
            }

            await _unitOfWork.SaveAsync();
            response = "success";
            return response;

        }
    }
}