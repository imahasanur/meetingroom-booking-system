using DotNetEnv;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Models.Room;

namespace RoomBooking.Models.Booking
{
    public class GetAllBookingViewModel
    {
        private IBookingManagementService _bookingService;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string State { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CreatedBy { get; set; }
        public string Host { get; set; }
        public Guid RoomId { get; set; }
        public List<Guest> Guests { get; set; }
        public RoomBooking.Application.Domain.Entities.Room Room { get; set; }
        public DateTime CreatedAtUTC { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _bookingService = provider.GetService<IBookingManagementService>();
        }

        public async Task<IList<ScheduleEvent>> GetAllEventAsync(DateTime start, DateTime end)
        {
            var allEvents = await _bookingService.GetAllEventAsync(start, end,null);
            var events = allEvents.Select(x => new ScheduleEvent { Id = x.Id, Start = x.Start, End = x.End, Resource = x.RoomId, Text = x.Name , Color = x.Color }).ToList();

            return events;

        }

        public async Task<IList<GetAllBookingViewModel>> GetAllEventAsync(string user)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddYears(1);

            var allEvents = await _bookingService.GetAllEventAsync(start, end, user);
            var getAllBooking = allEvents.Select(x => new GetAllBookingViewModel {Id = x.Id, Start = x.Start, End = x.End, Color = x.Color, CreatedBy = x.CreatedBy, State = x.State, Host = x.Host, Guests = x.Guests, RoomId = x.RoomId, Room = x.Room }).ToList();
            return getAllBooking;
        }

    }

    public class ScheduleEvent
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Guid Resource { get; set; }
        public string Color { get; set; }
        
    }
}
