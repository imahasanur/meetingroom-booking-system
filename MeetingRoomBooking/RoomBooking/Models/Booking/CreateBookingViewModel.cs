using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Models.Room;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace RoomBooking.Models.Booking
{
    public class CreateBookingViewModel
    {
        private IRoomManagementService _roomService;
        private IBookingManagementService _bookingService;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public string State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public Guid RoomId { get; set; }
        public string Guests { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public string Repeat { get; set; }
        public DateTime DateRange { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
            _bookingService = provider.GetService<IBookingManagementService>();
        }

        public async Task<List<RoomColumn>> LoadRoomAsync()
        {
            var rooms = await _roomService.LoadRoomAsync();
            var roomColumns = rooms.Select(x => new RoomColumn{ Name = $"{x.Location} {x.Name} ({x.Capacity}[{x.MinimumCapacity},{x.MaximumCapacity}])", Id = x.Id, Color = x.Color }).ToList();

            return roomColumns;
        }

        public async Task<string> CreateBookingAsync(CreateBookingViewModel model, IList<string> allUser, string userClaim)
        {

            var end = model.End.TimeOfDay;

            if(end.TotalHours == 0)
            {
                model.End = model.End.AddMinutes(-1);
            }

            var guests = new List<Guest>();
            var allGuest = model.Guests.Split(',').ToList();

            var bookingEvent = new CreateEventDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Start = model.Start,
                End = model.End,
                CreatedBy = model.CreatedBy,
                Host = model.Host,
                Description = model.Description,
                Color = model.Color,
                FontColor = model.FontColor,
                CreatedAtUTC = model.CreatedAtUTC,
                State = model.State,
                RoomId = model.RoomId,
                Guests = guests,
                Repeat = model.Repeat,
            };

            if (model.Repeat.Equals("1"))
            {
                var dateRange = model.DateRange.Date;
                var currentDate = DateTime.Now.Date;
                var eventDate = model.Start.Date;

                if (eventDate == dateRange)
                {
                    bookingEvent.Repeat = "0";
                }
                else if (dateRange < eventDate && dateRange >= currentDate)
                {
                    var difference = dateRange - eventDate;
                    var days = difference.Days;
                    bookingEvent.Repeat = days.ToString();
                }
                else if (dateRange > eventDate)
                {
                    var difference = dateRange - eventDate;
                    var days = difference.Days;
                    bookingEvent.Repeat = days.ToString();
                }
                else if (dateRange < currentDate)
                {
                    string result = string.Empty;
                    result = "Selected Date Range should not be less than current Date";

                    return result;
                }
            }

            var repeatedDays = Convert.ToInt32(bookingEvent.Repeat);

            if(repeatedDays < 0)
            {
                bookingEvent.Start = bookingEvent.Start.AddDays(repeatedDays);
                bookingEvent.End = bookingEvent.End.AddDays(repeatedDays);
                bookingEvent.Repeat = ((-1) * repeatedDays).ToString();
            }

            var response = await _bookingService.CreateBookingAsync(bookingEvent, allUser, userClaim, allGuest);

            return response;
        }
    }

    public class RoomColumn
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string Color { get; set; }
    }
}
