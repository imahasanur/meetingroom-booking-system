using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;
using QRCoder;

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

        [Required]
        [DataType(DataType.Text)]
        public string Color { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FontColor { get; set; }

        public string? CreatedBy { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MinimumCapacity { get; set; }

        [Range(2, 25, ErrorMessage = "The Room must hold at least 2 and at max 25 people")]
        public int? MaximumCapacity { get; set; }

        public string? QRCode { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? RoomImage { get; set; }


        public IList<GetRoomDTO>? PreviousRooms { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        
        public string QRCodeGeneration(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                string qrImage = Convert.ToBase64String(qrCodeImage);

                return $"data:image/png;base64,{qrImage}";
            }
        }

        public async Task<string> CreateRoomAsync(CreateRoomViewModel model)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var ms = new MemoryStream();

                model.ImageFile.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                model.RoomImage = Convert.ToBase64String(fileBytes);
            }

            var room = new CreateRoomDTO()
            { 
                Name = model.Name,
                Location = model.Location,
                Capacity = model.Capacity,
                CreatedAtUTC = DateTime.UtcNow,
                CreatedBy = model?.CreatedBy,
                Details = model.Details,
                Color = model.Color,
                MaximumCapacity = model?.MaximumCapacity ?? 0, 
                MinimumCapacity = model?.MinimumCapacity ?? 0,
                FontColor = model.FontColor,
                QRCode = model.QRCode,
                RoomImage = model.RoomImage,
            };

            var response = await _roomService.CreateRoomAsync(room);
            return response;
        }
    }
}
