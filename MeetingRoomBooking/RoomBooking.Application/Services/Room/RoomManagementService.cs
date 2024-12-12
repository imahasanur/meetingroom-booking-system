using RoomBooking.Application.DTO;
using RoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Application.Services.Room
{
    public class RoomManagementService:IRoomManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        
        public RoomManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var room = new Room()
            {
                Name = roomDTO.Name,
                Details = roomDTO.Details,
                Location = roomDTO.Location,
                Capacity = roomDTO.Capacity,
                CreatedAtUTC = roomDTO.CreatedAtUTC,
                CreatedBy = roomDTO.CreatedBy,
            };
            await _unitOfWork.RoomRepository.CreateRoomAsync(room);
            await _unitOfWork.SaveAsync();
        }
    }
}
