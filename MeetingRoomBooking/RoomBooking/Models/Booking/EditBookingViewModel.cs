using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Models.Room;

namespace RoomBooking.Models.Booking
{
    public class EditBookingViewModel
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
        public string Guests { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _bookingService = provider.GetService<IBookingManagementService>();
        }

        public async Task<string> EditBookingAsync(EditBookingViewModel model)
        {
            var eventDTO = new EditEventDTO()
            {
                Id = model.Id,
                Start = model.Start,
                End = model.End,
                RoomId = model.RoomId,
            };

            var response = await _bookingService.EditBookingAsync(eventDTO);

            return response;
        }
    }
}
