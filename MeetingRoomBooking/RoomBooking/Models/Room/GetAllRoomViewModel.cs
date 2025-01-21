using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Room
{
    public class GetAllRoomViewModel
    {
        private IRoomManagementService _roomService;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Details { get; set; }
        public string CreatedBy { get; set; }
        public int? MinimumCapacity { get; set; }
        public int? MaximumCapacity { get; set; }
        public string? UserClaim { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }
        public string? RoomImage { get; set; }
        public string? QRCode { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<IList<GetAllRoomViewModel>> LoadRoomAsync(string userClaim)
        {
            var rooms = await _roomService.LoadRoomAsync();
            var roomsViewModel = new List<GetAllRoomViewModel>();
            foreach(var room in rooms)
            {
                var viewModel = new GetAllRoomViewModel
                {
                    Id = room.Id,
                    Name = room.Name,
                    Location = room.Location,
                    Capacity = room.Capacity,
                    Details = room.Details,
                    CreatedBy = room.CreatedBy,
                    Color = room.Color,
                    FontColor = room.FontColor,
                    CreatedAtUTC = room.CreatedAtUTC,
                    LastUpdatedAtUTC = room.LastUpdatedAtUTC,
                    MinimumCapacity = room.MinimumCapacity,
                    MaximumCapacity = room.MaximumCapacity,
                    UserClaim = userClaim,
                    QRCode = room.QRCode,
                    RoomImage = room.RoomImage,
                };
                roomsViewModel.Add(viewModel);
            };

            return roomsViewModel;
        }
    }
}
