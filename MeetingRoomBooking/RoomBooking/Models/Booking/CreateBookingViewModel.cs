using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Models.Room;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Booking
{
    public class CreateBookingViewModel
    {
        private IRoomManagementService _roomService;

        public string Name { get; set; }
        public string Color { get; set; }
        public string State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public string Host { get; set; }
        public Guid RoomId { get; set; }
        public string Guests { get; set; }
        public DateTime CreatedAtUTC { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        public async Task<List<RoomColumn>> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomAsync();
            var roomColumns = rooms.Select(x => new RoomColumn{ Name = $"{x.Location} {x.Name}", Id = x.Id }).ToList();

            return roomColumns;
        }

    }

    public class RoomColumn
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
