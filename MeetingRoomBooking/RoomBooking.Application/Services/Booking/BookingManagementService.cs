using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Data.Common;

namespace RoomBooking.Application.Services.Booking
{
    public class BookingManagementService:IBookingManagementService
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

        //public async Task<IList<GetRoomDTO>> GetAllRoomAsync()
        //{
        //    var rooms = await _unitOfWork.RoomRepository.GetAllRoomAsync();
        //    var roomsDTO = new List<GetRoomDTO>();

        //    for(int i = 0; i < rooms.Count; i++)
        //    {
        //        var room = rooms[i];
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
        //        };
        //        roomsDTO.Add(roomDTO);
        //    }
        //    return roomsDTO;
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
            //    var rooms = await _unitOfWork.RoomRepository.CheckRoomRedundancy(roomDTO.Location, roomDTO.Name);
            //    if (rooms is not null && rooms?.Count > 0)
            //    {
            //        response = "redundant";
            //        return response;
            //    }

            //    await _unitOfWork.RoomRepository.CreateRoomAsync(room);
            //    await _unitOfWork.SaveAsync();

            //    response = "success";

            //    return response;
            //}

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



            //public async Task<string> EditRoomAsync(EditRoomDTO roomDTO)
            //{
            //    string response = "";
            //    try
            //    {
            //        var existingRoom = await _unitOfWork.RoomRepository.GetRoomAsync(roomDTO.Id);

            //        if (existingRoom.CreatedBy is not null)
            //        {
            //            var rooms = await _unitOfWork.RoomRepository.CheckRoomRedundancy(roomDTO.Id, roomDTO.Location, roomDTO.Name);
            //            if (rooms is not null && rooms?.Count > 0)
            //            {
            //                response = "redundant";
            //                return response;
            //            }

            //            existingRoom.Location = roomDTO.Location;
            //            existingRoom.Capacity = roomDTO.Capacity;
            //            existingRoom.Details = roomDTO.Details;
            //            existingRoom.LastUpdatedAtUTC = DateTime.UtcNow;
            //            existingRoom.Name = roomDTO.Name;
            //            existingRoom.ConcurrencyToken = Guid.NewGuid();
            //        }

            //        await _unitOfWork.RoomRepository.EditRoomAsync(existingRoom);
            //        await _unitOfWork.SaveAsync();

            //        response = "success";
            //        return response;
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        response = ex.Message;
            //        return response;

            //    }
            //    catch (Exception ex) 
            //    {
            //        response = ex.Message;
            //        return response;
            //    }

            //}
        }
    }