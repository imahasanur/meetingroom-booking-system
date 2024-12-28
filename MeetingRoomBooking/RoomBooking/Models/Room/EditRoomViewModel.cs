using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Room
{
    public class EditRoomViewModel
    {
        private IRoomManagementService _roomService;

        public Guid Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Room Name")]
        [RegularExpression(@"^[A-B][1-3]$", ErrorMessage = "One char from A/B and one char from 1/2/3")]
        [Required(ErrorMessage = "Accepted Combination [A1, B1, A2, B2, A3, B3] ..")]
        public string Name { get; set; }  

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Location")]
        public string Location { get; set; }

        [Range(3, 25 ,ErrorMessage = "The Room must hold at least 3 and at max 25 people")]
        [Display(Name = "Room Capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Add some details about meeting room")]
        [DataType(DataType.Text)]
        [Display(Name = "Room Details")]
        public string Details { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Color { get; set; }   
        public string CreatedBy { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MinimumCapacity { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MaximumCapacity { get; set; }

        public Guid ConcurrencyToken { get; set; }

        public IList<GetRoomDTO>? PreviousRooms { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<EditRoomViewModel> GetRoomAsync(Guid id)
        {
            var room = await _roomService.GetRoomAsync(id);

            if (room is null || room?.CreatedBy is null)
            {
                var roomViewModel = new EditRoomViewModel();
                return roomViewModel;
            }
            var viewModel = new EditRoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Location = room.Location,
                Capacity = room.Capacity,
                Details = room.Details,
                CreatedBy = room.CreatedBy,
                Color = room.Color,
                CreatedAtUTC = room.CreatedAtUTC,
                MaximumCapacity = room.MaximumCapacity,
                MinimumCapacity = room.MinimumCapacity,
                LastUpdatedAtUTC = room.LastUpdatedAtUTC,
                ConcurrencyToken = room.ConcurrencyToken
            };

            return viewModel;
        }

        public async Task<EditRoomViewModel> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomAsync();
            var model = new EditRoomViewModel()
            {
                PreviousRooms = rooms
            };
            return model;
        }

        public async Task<string> EditRoomAsync(EditRoomViewModel model)
        {
            var room = new EditRoomDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                CreatedAtUTC = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                Details = model.Details,
                Color = model.Color,
                MaximumCapacity = model.MaximumCapacity,
                MinimumCapacity = model.MinimumCapacity,
                LastUpdatedAtUTC = DateTime.UtcNow,
                ConcurrencyToken = model.ConcurrencyToken
            };

            var response = await _roomService.EditRoomAsync(room);

            return response;
        }

        internal async Task GetEventByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
