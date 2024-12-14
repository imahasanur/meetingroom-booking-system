using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Domain.Entities;
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

        public async Task<GetRoomDTO?> GetRoomAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomAsync(id);

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
                };
                roomsDTO.Add(roomDTO);
            }
            return roomsDTO;
        }

        public async Task CreateRoomAsync(CreateRoomDTO roomDTO)
        {
            await _unitOfWork.RoomRepository.CreateRoomAsync(roomDTO);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteRoomAsync(RoomDTO roomDTO)
        {
            await _unitOfWork.RoomRepository.DeleteRoomAsync(roomDTO);
            await _unitOfWork.SaveAsync();
        }

        public async Task EditRoomAsync(EditRoomDTO roomDTO)
        {
            try
            {
                await _unitOfWork.RoomRepository.EditRoomAsync(roomDTO);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException ex) { 
                var exceptionEntry = ex.Entries.Single(); 

                var clientValues = (RoomBooking.Application.Domain.Entities.Room)exceptionEntry.Entity;
                var room = await _unitOfWork.RoomRepository.GetRoomAsync(clientValues.Id);

                room.Name = roomDTO.Name;
                room.Location = roomDTO.Location;
                room.CreatedBy = roomDTO.CreatedBy;
                room.LastUpdatedAtUTC = DateTime.UtcNow;
                room.Capacity = roomDTO.Capacity;
                room.Details = roomDTO.Details;
                _unitOfWork.RoomRepository.EditAsync(room);
                _unitOfWork.SaveAsync();


                //if (room.CreatedBy is not null) 
                //{
                //    await _unitOfWork.RoomRepository.EditRoomAsync(roomDTO);
                //    await _unitOfWork.SaveAsync();
                //}
                //else
                //{
                //    throw new NotImplementedException();
                //}

                

                // Retry the update
                //_unitOfWork.RoomRepository.EditRoomAsync2(databaseValues); // Assuming `Update` method exists in your repository
                //await _unitOfWork.SaveAsync();
            }

        }
    }
}
