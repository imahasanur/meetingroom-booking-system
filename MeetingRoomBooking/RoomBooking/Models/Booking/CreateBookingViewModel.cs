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
        private IBookingManagementService _bookingService;

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
            _bookingService = provider.GetService<IBookingManagementService>();
        }

        public async Task<List<RoomColumn>> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomAsync();
            var roomColumns = rooms.Select(x => new RoomColumn{ Name = $"{x.Location} {x.Name}", Id = x.Id }).ToList();

            return roomColumns;
        }

        public async Task<string> CreateBookingAsync(CreateBookingViewModel model)
        {

            var guests = new List<Guest>();
            var temp = model.Guests.Split(',');
            var id = Guid.NewGuid();

            for (int i = 0; i < temp.Length; i++)
            {
                var guest = temp[i].Trim();
                guests.Add(new Guest
                {
                    Id = Guid.NewGuid(),
                    EventId = id,
                    User = guest,
                    CreatedAtUTC = DateTime.UtcNow,
                });
            }

            var bookingEvent = new CreateEventDTO()
            {
                Id = id,
                Name = model.Name,
                Start = model.Start,
                End = model.End,
                CreatedBy = model.CreatedBy,
                Host = model.Host,
                Color = model.Color,
                CreatedAtUTC = model.CreatedAtUTC,
                State = model.State,
                RoomId = model.RoomId,
                Guests = guests,
            };

            var response = await _bookingService.CreateBookingAsync(bookingEvent);

            return response;
        }

    }

    public class RoomColumn
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
