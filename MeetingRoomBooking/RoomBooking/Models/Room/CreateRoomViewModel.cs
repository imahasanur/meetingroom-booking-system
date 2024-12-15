using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Room
{
    public class CreateRoomViewModel
    {
        private IRoomManagementService _roomService;

        public Guid? Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Room Name")]
        [RegularExpression(@"^[A - B]{1}[1 - 2]{1}",ErrorMessage ="One char from A/B and one char from 1/2")]
        [Required(ErrorMessage = "Accepted Combination [A1, B1, A2, B2] ..")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Location")]
        public string Location { get; set; }

        [Range(3, 15, ErrorMessage = "The Room must hold at least 3 and at max 15 people")]
        [Display(Name = "Room Capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Add some details about meeting room")]
        [DataType(DataType.Text)]
        [Display(Name = "Room Details")]
        public string Details { get; set; }
        public string? CreatedBy { get; set; }

        public IList<GetRoomDTO>? PreviousRooms { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<CreateRoomViewModel> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomAsync();
            var model = new CreateRoomViewModel()
            {
                PreviousRooms = rooms
            };
            return model;
        }

        public async Task<string> CreateRoomAsync(CreateRoomViewModel model)
        {
            var room = new CreateRoomDTO()
            { 
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                CreatedAtUTC = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                Details = model.Details
            };
            var response = await _roomService.CreateRoomAsync(room);
            return response;
        }
    }
}
