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
        [RegularExpression(@"^[A-B][1-3]$", ErrorMessage ="One char from A/B and one char from 1/2/3")]
        [Required(ErrorMessage = "Accepted Combination [A1, B1, A2, B2, A3, B3,..] ..")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Location")]
        public string Location { get; set; }

        [Range(5, 25, ErrorMessage = "The Room capacity will at least 5 and at max 25 people")]
        [Display(Name = "Room Capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Add some details about meeting room")]
        [DataType(DataType.Text)]
        [Display(Name = "Room Details")]
        public string Details { get; set; }
        public string? CreatedBy { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MinimumCapacity { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MaximumCapacity { get; set; }

        public IList<GetRoomDTO>? PreviousRooms { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<string> CreateRoomAsync(CreateRoomViewModel model)
        {
            var room = new CreateRoomDTO()
            { 
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                CreatedAtUTC = DateTime.UtcNow,
                CreatedBy = model?.CreatedBy,
                Details = model.Details,
                MaximumCapacity = model?.MaximumCapacity ?? 0, 
                MinimumCapacity = model?.MinimumCapacity ?? 0,
            };
            var response = await _roomService.CreateRoomAsync(room);
            return response;
        }
    }
}
