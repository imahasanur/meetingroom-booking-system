﻿using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Data.Common;

namespace RoomBooking.Application.Services.Room
{
    public class RoomManagementService:IRoomManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        
        public RoomManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        (string,bool) ValidateAttendeeLimit(int? maximumCapacity, int? minimumCapacity, int capacity)
        {
            bool isValid = true;
            string response = string.Empty;

            if (maximumCapacity != 0 && minimumCapacity != 0 && minimumCapacity >= maximumCapacity)
            {
                response = "Minimum room attendee can't be equal or greater than Maximum attendee number";
                isValid = false;

                return (response,isValid);
            }
            else if (maximumCapacity != 0 && minimumCapacity != 0 && maximumCapacity > capacity)
            {
                response = "Maximum attendee can't cross room capacity";
                isValid = false;

                return (response,isValid);
            }
            else if (maximumCapacity == 0 && minimumCapacity != 0 && minimumCapacity > capacity)
            {
                response = "Room minimum attendee can not cross room total attendee capacity.";
                isValid = false;

                return (response, isValid);
            }
            else if (maximumCapacity != 0 && minimumCapacity == 0 && maximumCapacity > capacity)
            {
                response = "Room Maximum attendee can not cross room attendee capacity";
                isValid = false;

                return (response, isValid);
            }
            
            return (response, isValid);
        }

        public async Task<GetRoomDTO?> GetRoomAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomAsync(id,true);

            if (room is not null)
            {
                var roomDTO = new GetRoomDTO()
                {
                    Capacity = room.Capacity,
                    CreatedAtUTC = room.CreatedAtUTC,
                    Location = room.Location,
                    Id = room.Id,
                    Name = room.Name,
                    Details = room.Details,
                    LastUpdatedAtUTC = room.LastUpdatedAtUTC,
                    CreatedBy = room.CreatedBy,
                    MaximumCapacity = room.MaximumCapacity,
                    MinimumCapacity = room.MinimumCapacity,
                    ConcurrencyToken = room.ConcurrencyToken
                };

                return roomDTO;
            }

            return null;
        }

        public async Task<IList<GetRoomDTO>> GetAllRoomAsync()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllRoomAsync();
            var roomsDTO = new List<GetRoomDTO>();

            for(int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                var roomDTO = new GetRoomDTO() 
                { 
                    Capacity = room.Capacity,
                    CreatedAtUTC = room.CreatedAtUTC,
                    Location = room.Location,
                    Id = room.Id,
                    Name = room.Name,
                    Details = room.Details,
                    LastUpdatedAtUTC = room.LastUpdatedAtUTC,
                    CreatedBy = room.CreatedBy,
                    MinimumCapacity = room.MinimumCapacity,
                    MaximumCapacity = room.MaximumCapacity,
                };
                roomsDTO.Add(roomDTO);
            }
            return roomsDTO;
        }

        public async Task<string> CreateRoomAsync(CreateRoomDTO roomDTO)
        {
            var room = new RoomBooking.Application.Domain.Entities.Room()
            {
                Name = roomDTO.Name,
                Details = roomDTO.Details,
                Location = roomDTO.Location,
                Capacity = roomDTO.Capacity,
                CreatedAtUTC = roomDTO.CreatedAtUTC,
                CreatedBy = roomDTO.CreatedBy,
                MinimumCapacity = roomDTO.MinimumCapacity,
                MaximumCapacity = roomDTO.MaximumCapacity,
                ConcurrencyToken = Guid.NewGuid()
            };

            string response = string.Empty;

            var result = ValidateAttendeeLimit(room.MaximumCapacity, room.MinimumCapacity, room.Capacity);

            if(result.Item2 == false)
            {
                response = result.Item1;

                return response;
            }
            
            var rooms = await _unitOfWork.RoomRepository.CheckRoomRedundancy(roomDTO.Location, roomDTO.Name);
            if (rooms is not null && rooms?.Count > 0)
            {
                response = "redundant";
                return response;
            }

            await _unitOfWork.RoomRepository.CreateRoomAsync(room);
            await _unitOfWork.SaveAsync();

            response = "success";

            return response;
        }

        public async Task<string> DeleteRoomAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomAsync(id, true);

            string response = string.Empty;

            if(room == null || room?.CreatedBy == null)
            {
                response = "Room not found , May be deleted.";
                return response;
            }
            await _unitOfWork.RoomRepository.DeleteRoomAsync(room);
            await _unitOfWork.SaveAsync();

            response = "success";

            return response;
        }


        public async Task<string> EditRoomAsync(EditRoomDTO roomDTO)
        {
            string response = string.Empty;

            try
            {
                var existingRoom = await _unitOfWork.RoomRepository.GetRoomAsync(roomDTO.Id, true);

                if (existingRoom.CreatedBy is not null)
                {
                    var rooms = await _unitOfWork.RoomRepository.CheckRoomRedundancy(roomDTO.Id, roomDTO.Location, roomDTO.Name);
                    if (rooms is not null && rooms?.Count > 0)
                    {
                        response = "redundant";
                        return response;
                    }

                    existingRoom.Location = roomDTO.Location;
                    existingRoom.Capacity = roomDTO.Capacity;
                    existingRoom.Details = roomDTO.Details;
                    existingRoom.LastUpdatedAtUTC = DateTime.UtcNow;
                    existingRoom.Name = roomDTO.Name;
                    existingRoom.MaximumCapacity = roomDTO.MaximumCapacity;
                    existingRoom.MinimumCapacity = roomDTO.MinimumCapacity;
                    existingRoom.ConcurrencyToken = Guid.NewGuid();


                    var result = ValidateAttendeeLimit(existingRoom.MaximumCapacity, existingRoom.MinimumCapacity, existingRoom.Capacity);

                    if (result.Item2 == false)
                    {
                        response = result.Item1;

                        return response;
                    }
                }

                await _unitOfWork.RoomRepository.EditRoomAsync(existingRoom);
                await _unitOfWork.SaveAsync();

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