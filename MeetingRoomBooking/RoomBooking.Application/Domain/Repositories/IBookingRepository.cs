﻿using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;

namespace RoomBooking.Application.Domain.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Event, Guid>
    {
        //Task<IList<Room>?> CheckRoomRedundancy(Guid id, string location, string name);
        //Task<IList<Room>> CheckRoomRedundancy(string location, string name);
        Task<Event> GetEventAsync(Guid id);
        Task<IList<Event>> GetAllEventAsync(DateTime start, DateTime end);
        Task CreateBookingAsync(Event eventEntity);
        Task EditBookingAsync(Event eventEntity);
        //Task DeleteRoomAsync(Room room);
    }
}