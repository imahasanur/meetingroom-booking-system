using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Application.DTO;
using System.Web.Mvc;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application.Services.Room;

namespace RoomBooking.Models.Room
{
    public class DeleteRoomViewModel
    {
        private IRoomManagementService _roomService;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Details { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime? LastUpdatedAtUTC { get; set; }

        public void ResolveDI(IServiceProvider provider)
        {
            _roomService = provider.GetService<IRoomManagementService>();
        }

        //public async Task<DeleteRoomViewModel> GetRoomAsync(Guid id)
        //{
        //    var room = await _roomService.GetRoomAsync(id);

        //    if (room is null || room?.CreatedBy is null)
        //    {
        //        var roomViewModel = new DeleteRoomViewModel();
        //        return roomViewModel;
        //    }
        //    var viewModel = new DeleteRoomViewModel
        //    {
        //        Id = room.Id,
        //        Name = room.Name,
        //        Location = room.Location,
        //        Capacity = room.Capacity,
        //        Details = room.Details,
        //        CreatedBy = room.CreatedBy,
        //        CreatedAtUTC = room.CreatedAtUTC,
        //        LastUpdatedAtUTC = room.LastUpdatedAtUTC
        //    };

        //    return viewModel;
        //}

        public async Task<string> DeleteRoomAsync(Guid id)
        {
            string response = string.Empty;

            response = await _roomService.DeleteRoomAsync(id);

            return response;
        }
    }
}
