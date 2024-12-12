using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;

namespace RoomBooking.Models.Room
{
    public class RoomCreateModel
    {
        public Guid? Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Room Capacity")]
        public int Capacity { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Room Details")]
        public string Details { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CreatedBy { get; set; }

        public IList<GetRoomDTO> PreviousRooms { get; set; }


    }
}
