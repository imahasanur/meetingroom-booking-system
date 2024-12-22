using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Room
{
    public class SetLimitRoomViewModel
    {
        private IRoomManagementService _roomService;

        public Guid Id { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "The Room must hold at least 1 and at max 20 people")]
        public int MinimumCapacity { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "The Room must hold at least 1 and at max 20 people")]
        public int MaximumCapacity { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }

        public IList<GetRoomDTO>? PreviousRooms { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<SetLimitRoomViewModel> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomAsync();
            var model = new SetLimitRoomViewModel()
            {
                PreviousRooms = rooms
            };
            return model;
        }


        public async Task<string> EditRoomAsync(SetLimitRoomViewModel model)
        {
            var room = new EditRoomSettingDTO()
            {
                Id = model.Id,
                MinimumCapacity = model.MinimumCapacity,
                MaximumCapacity = model.MaximumCapacity,
                LastUpdatedAtUTC = DateTime.UtcNow,
            };

            var response = await _roomService.EditRoomSettingAsync(room);

            return response;
        }

    }
}

