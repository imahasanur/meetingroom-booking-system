using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Models.Booking;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.Booking
{
    public class EditBookingViewModel
    {

        private IBookingManagementService _bookingService;

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
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

        public async Task<string> EditBookingByIdAsync(EditBookingViewModel model)
        {
            var eventDTO = new EditEventDTO()
            {
                Id = model.Id,
                Name = model.Name,
                State = model.State,
                Start = model.Start,
                End = model.End,
                Host = model.Host,
                AllGuest = model.Guests
            };

            var response = await _bookingService.EditBookingByIdAsync(eventDTO);

            return response;
        }
        
        public async Task<EditBookingViewModel> GetEventByIdAsync(Guid id)
        {
            
            var eventDTO = await _bookingService.GetEventByIdAsync(id); 

            if(eventDTO == null)
            {
                return null;
            }

            string guests = string.Empty;
            guests = string.Join(", ",eventDTO.Guests.Select(x => x.User));

            var editBookingModel = new EditBookingViewModel
            {
                Id = eventDTO.Id,
                Name = eventDTO.Name,
                Color = eventDTO.Color,
                State = eventDTO.State,
                Start = eventDTO.Start,
                End = eventDTO.End,
                CreatedBy = eventDTO.CreatedBy,
                Host = eventDTO.Host,
                Guests = guests
            };
            return editBookingModel;
        }
    }
}
