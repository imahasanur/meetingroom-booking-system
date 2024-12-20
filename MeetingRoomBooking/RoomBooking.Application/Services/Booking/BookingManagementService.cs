using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Data.Common;

namespace RoomBooking.Application.Services.Booking
{
    public class BookingManagementService : IBookingManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        
        public BookingManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<GetRoomDTO?> GetRoomAsync(Guid id)
        //{
        //    var room = await _unitOfWork.RoomRepository.GetRoomAsync(id);

        //    if (room is not null)
        //    {
        //        var roomDTO = new GetRoomDTO()
        //        {
        //            Capacity = room.Capacity,
        //            CreatedAtUTC = room.CreatedAtUTC,
        //            Location = room.Location,
        //            Id = room.Id,
        //            Name = room.Name,
        //            Details = room.Details,
        //            LastUpdatedAtUTC = room.LastUpdatedAtUTC,
        //            CreatedBy = room.CreatedBy,
        //            ConcurrencyToken = room.ConcurrencyToken
        //        };
        //        return roomDTO;
        //    }

        //    return null;
        //}

      

        public async Task<string> CreateBookingAsync(CreateEventDTO eventDTO)
        {
            string response = string.Empty;

            var eventEntity = new RoomBooking.Application.Domain.Entities.Event()
            {
                Id = eventDTO.Id,
                Name = eventDTO.Name,
                Color = eventDTO.Color,
                State = eventDTO.State,
                Start = eventDTO.Start,
                End = eventDTO.End,
                CreatedAtUTC = eventDTO.CreatedAtUTC,
                CreatedBy = eventDTO.CreatedBy,
                Host = eventDTO.Host,
                RoomId = eventDTO.RoomId,
                Guests = eventDTO.Guests,
            };


            await _unitOfWork.BookingRepository.CreateBookingAsync(eventEntity);
            await _unitOfWork.SaveAsync();


            response = "success";

            return response;
        }
        public async Task<IList<GetEventDTO>> GetAllEventAsync(DateTime start, DateTime end, string? user)
        {
            var allEvent = await _unitOfWork.BookingRepository.GetAllEventAsync(start, end, user);
            var eventsDTO = new List<GetEventDTO>();

            for (int i = 0; i < allEvent.Count; i++)
            {
                var scheduleEvent = allEvent[i];
                var eventDTO = new GetEventDTO()
                {
                    Id = scheduleEvent.Id,
                    Name = scheduleEvent.Name,
                    Color = scheduleEvent.Color,
                    State = scheduleEvent.State,
                    Start = scheduleEvent.Start,
                    End = scheduleEvent.End,
                    RoomId= scheduleEvent.RoomId,
                    CreatedBy= scheduleEvent.CreatedBy,
                    Host = scheduleEvent.Host,
                    Guests= scheduleEvent.Guests,
                    Room = scheduleEvent.Room,
                };
                eventsDTO.Add(eventDTO);
            }
            return eventsDTO;

        }

        //public async Task DeleteRoomAsync(RoomDTO roomDTO)
        //{
        //    var room = new RoomBooking.Application.Domain.Entities.Room()
        //    {
        //        Name = roomDTO.Name,
        //        Details = roomDTO.Details,
        //        Location = roomDTO.Location,
        //        Capacity = roomDTO.Capacity,
        //        CreatedAtUTC = roomDTO.CreatedAtUTC,
        //        CreatedBy = roomDTO.CreatedBy,
        //    };
        //    await _unitOfWork.RoomRepository.DeleteRoomAsync(room);
        //    await _unitOfWork.SaveAsync();
        //}
        public async Task<string> DeleteBookingAsync(Guid id)
        {
            string response = string.Empty;

            var eventEntity = await _unitOfWork.BookingRepository.GetBookingAsync(id);

            if(eventEntity.CreatedBy is null)
            {
                response = "not found";
                return response;
            }

            await _unitOfWork.BookingRepository.DeleteBookingAsync(eventEntity);
            await _unitOfWork.SaveAsync();

            response = "success";

            return response;
        }


        public async Task<string> EditBookingAsync(EditEventDTO eventDTO)
        {
            string response = string.Empty;
            try
            {
                var existingEvent = await _unitOfWork.BookingRepository.GetEventAsync(eventDTO.Id);
                if (existingEvent.CreatedBy != null)
                {
                    existingEvent.Start = eventDTO.Start;
                    existingEvent.End = eventDTO.End;
                    existingEvent.RoomId = eventDTO.RoomId;

                    await _unitOfWork.BookingRepository.EditBookingAsync(existingEvent);
                    await _unitOfWork.SaveAsync();
                }
                response = "success";
                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                response = ex.Message;
                return response;

            }
            catch (Exception ex)
            {
                response = ex.Message;
                return response;
            }
        }
    }
}