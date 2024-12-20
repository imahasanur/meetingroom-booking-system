using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Booking
{
    public class DeleteBookingViewModel
    {
        private IBookingManagementService _bookingService;

        public Guid Id { get; set; }
      
        public void ResolveDI(IServiceProvider provider)
        {
            _bookingService = provider.GetService<IBookingManagementService>();
        }

        public async Task<string> DeleteBookingAsync(Guid id)
        {
            var response = await _bookingService.DeleteBookingAsync(id);
            return response;
        }

    }
}
