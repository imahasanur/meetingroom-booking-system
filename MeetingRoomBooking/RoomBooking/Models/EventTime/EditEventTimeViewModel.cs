using RoomBooking.Application.DTO;
using RoomBooking.Application.Services.EventTime;
using RoomBooking.Application.Services.Room;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models.EventTime
{
    public class EditEventTimeViewModel
    {
        private IEventTimeManagementService _timeService;

        public Guid Id { get; set; }

        [Required]
        [Range(15, 60,ErrorMessage ="Time min vlaue is 15 minutes to 60 minutes")]
        public int MinimumTime { get; set; }
        [Required]
        public int MaximumTime { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? UpdatedBy { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _timeService = provider.GetService<IEventTimeManagementService>();
        }

        public async Task<EditEventTimeViewModel> GetEventTimeLimitAsync()
        {
            var eventTimeDTO = await _timeService.GetTimeLimitAsync();
            var eventTimeModel = new EditEventTimeViewModel
            {
                Id = eventTimeDTO.Id,
                MinimumTime = eventTimeDTO.MinimumTime,
                MaximumTime = eventTimeDTO.MaximumTime,
            };

            return eventTimeModel;
        }

        public async Task<string> EditEventTimeLimitAsync(EditEventTimeViewModel model)
        {
            var response = string.Empty;

            var eventTimeDTO = new EditEventTimeDTO { Id = model.Id,
                MinimumTime = model.MinimumTime,
                MaximumTime = model.MaximumTime,
                LastUpdatedAtUTC=DateTime.UtcNow,
                UpdatedBy = model.UpdatedBy,
            };
            
            response = await _timeService.EditEventTimeLimitAsync(eventTimeDTO);
            return response;
        }
    }
}
