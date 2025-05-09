﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace RoomBooking.Application.Services.Booking
{
    public class BookingManagementService : IBookingManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public BookingManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        int ConvertTimeToMinutes(DateTime start, DateTime end)
        {
            var hourDifference = end - start;
            var minuteDifference = (int)hourDifference.TotalMinutes;
            return minuteDifference;
        }

        (bool,string) CheckBookingAttendeeLimit(int? maximumCapacity, int? minimumCapacity, int capacity, int members)
        {
            bool isValid = true;
            string response = string.Empty;
            
            if(maximumCapacity == 0 && minimumCapacity == 0 && (members > capacity))
            {
                isValid = false;
            }
            else if(maximumCapacity != 0 && minimumCapacity == 0 && (!(members  <= maximumCapacity && members <= capacity)))
            {
                isValid = false;
                
            }
            else if(maximumCapacity == 0 && minimumCapacity != 0 && (members < minimumCapacity && members <= capacity))
            {
                isValid = false;
            }
            else if(maximumCapacity != 0 && minimumCapacity != 0 && (members < minimumCapacity || (members > maximumCapacity ) || members > capacity))
            {
                isValid = false;
            }

            return (isValid, response);
        }

        (bool,string) ValidateEventTimeLimit(DateTime start, DateTime end, IList<RoomBooking.Domain.Domain.Entities.EventTime> eventTimeEntity)
        {
            var meetingTimeDifference = ConvertTimeToMinutes(start, end);
           
            string response = string.Empty;
            bool isValid = true;

            if (eventTimeEntity != null && eventTimeEntity.Count > 0)
            {
                var maximumLimit = eventTimeEntity[0].MaximumTime;
                var minimumLimit = eventTimeEntity[0].MinimumTime;

                if (meetingTimeDifference > maximumLimit)
                {
                    response = "Event crosses Maximum Time Limit";
                    isValid = false;

                    return (isValid, response);
                }
                else if (meetingTimeDifference < minimumLimit)
                {

                    response = "Event is less than Minimum Time Limit";
                    isValid = false;

                    return (isValid, response);
                }
                else
                {
                    response = "Valid time limit";

                    return (isValid, response);
                }
            }
            else
            {
                response = "Event Time limit is not set; Set it first";
                isValid = false;

                return (isValid, response);
            }
        }

        (bool,string) ValidateMeetingAttendee(IList<string> guests, string host, IList<string> users, string bookingMaker, string userClaim)
        {
            bool isValid = true;
            string response = string.Empty;

            isValid = users.Contains(host);


            if (isValid == true)
            {
                var found = users.Intersect(guests).ToList();

                if (found.Count == guests.Count) 
                {
                    isValid = true;
                    response = "All guest are valid registered users";
                }
                else
                {
                    isValid = false;
                    response = "All guest are not valid registered users";
                }
            }
            else
            {
                response = "Host are not valid registered user";
                return (isValid, response);
            }

            // Check meeting creator in booking guests list or in host.
            if (isValid == true && userClaim == "user")
            {
                var hasFound = guests.Contains(bookingMaker);
                var isHost = host.Equals(bookingMaker) ? true : false;

                if (hasFound == true && isHost == true)
                {
                    isValid = false;
                    response = "Booking maker is a host and guest. Which should not be";
                }
                else if (hasFound == false && isHost == false)
                {
                    isValid = false;
                    response = "Booking maker not a host or guest";
                }
                else
                {
                    isValid = true;
                }
            }
            else if (isValid == true && userClaim == "admin") 
            {
                var hasFound = guests.Contains(bookingMaker);
                var isHost = host.Equals(bookingMaker) ? true : false;

                if(hasFound == true && isHost == true)
                {
                    isValid = false;
                }

                if (guests.Contains(host) == true)
                {
                    isValid = false;
                    response = "Host in guest list which should not be.";
                }
            }

            // Redundant guests check.
            var hash = new HashSet<string>();
            var duplicates = guests.Where(i => !hash.Add(i)).ToList();

            if (duplicates.Count > 0) { 
                isValid = false;
                response = "Guest repeated ";
            }

            return (isValid, response);
        }

        async Task<(bool, string)> CheckBookingConstraints(DateTime start, DateTime end, Guid roomId, string host, string createdBy)
        {
            bool isValid = true;
            var response = string.Empty;

            // Check user for same room , same day overlapping meeting.
            var bookings = await _unitOfWork.BookingRepository.CheckBookingOverlapping(start, end, roomId);

            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found same room , same day overlapping meeting.";

                return (isValid, response);
            }

            // Check user for different room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckAnyRoomBookingOverlappingByUser(start, end, createdBy);

            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found different room , same day overlapping meeting by a booking creator.";

                return (isValid, response);
            }

            // Check Host user for different room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckAnyRoomBookingOverlappingByHost(start, end, host);

            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found different room , same day overlapping meeting by a host.";

                return (isValid, response);
            }

            // Check event start time is backward or not.
            var isBackward = start > DateTime.Now ? true : false;

            if (isBackward == false)
            {
                isValid = false;
                response = "Event start time can't be set backward than current time while creatig";

                return (isValid, response);
            }

            return (isValid, response);
        }

        async Task<(bool,string)> CheckBookingEditConstraints(DateTime start, DateTime end, Guid roomId, Guid eventId, string createdBy, string host)
        {
            bool isValid = true;
            string response = string.Empty;

            // Check user for same room , same day overlapping meeting.
            var bookings = await _unitOfWork.BookingRepository.CheckEditBookingOverlapping(start, end, roomId, eventId);
            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found same room , same day overlapping meeting.";

                return (isValid,response);
            }

            // Check user for different room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckEditAnyRoomBookingOverlappingByUser(start, end,createdBy, eventId);
            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found different room , same day overlapping meeting by a booking creator.";

                return (isValid, response);
            }

            // Check Host for different room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckEditAnyRoomBookingOverlappingByHost(start, end, host, eventId);
            if (bookings != null && bookings.Count > 0)
            {
                isValid = false;
                response = "Found different room , same day overlapping meeting by a host.";

                return (isValid, response);
            }

            // Check event start time is backward or not.
            var isBackward = start > DateTime.Now ? true : false;
            if (isBackward == false)
            {
                isValid = false ;
                response = "Event start time can't be set backward than current time while updating";

                return (isValid, response);
            }

            return (isValid, response);
        }

        public async Task<string> CreateBookingAsync(CreateEventDTO eventDTO, IList<string> allUser, string userClaim, List<string> guests)
        {
            string response = string.Empty;

            var events = new List<Event>();

            try
            {
                var repeat = Convert.ToInt32(eventDTO.Repeat);

                for (int i = 0; i <= repeat; i++)
                {

                    var eventGuests = new List<Guest>();
                    var id = Guid.NewGuid();

                    for (int j = 0; j < guests.Count; j++)
                    {
                        var guest = guests[j].Trim();
                        eventGuests.Add(new Guest
                        {
                            Id = Guid.NewGuid(),
                            EventId = id,
                            User = guest,
                            CreatedAtUTC = DateTime.UtcNow,
                        });
                    }

                    var eventEntity = new RoomBooking.Domain.Domain.Entities.Event()
                    {
                        Id = id,
                        Name = eventDTO.Name,
                        Color = eventDTO.Color,
                        State = eventDTO.State,
                        Start = eventDTO.Start,
                        End = eventDTO.End,
                        Description = eventDTO.Description,
                        CreatedAtUTC = eventDTO.CreatedAtUTC,
                        CreatedBy = eventDTO.CreatedBy,
                        Host = eventDTO.Host,
                        RoomId = eventDTO.RoomId,
                        Guests = eventGuests,
                        
                    };

                    eventEntity.Start = eventDTO.Start.AddDays(i);
                    eventEntity.End = eventDTO.End.AddDays(i);

                    // Check meeting booking time limit with room maximum and minimum time.
                     var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
                    var isValid = ValidateEventTimeLimit(eventEntity.Start, eventEntity.End, eventTimeEntity);

                    var allGuest = eventEntity.Guests.Select(x => x.User.Trim()).ToList();

                    if (isValid.Item1 == false)
                    {
                        return isValid.Item2;
                    }

                    // Check meeting booking user & guests validity in users list.
                    isValid = ValidateMeetingAttendee(allGuest, eventEntity.Host, allUser, eventEntity.CreatedBy, userClaim);

                    if (isValid.Item1 == false)
                    {
                        return isValid.Item2;
                    }

                    // Check booking minimum and maximum room attendee limit.
                    var room = await _unitOfWork.RoomRepository.GetRoomAsync(eventEntity.RoomId, false);

                    if (room != null && room?.Capacity != 0)
                    {
                        isValid = CheckBookingAttendeeLimit(room.MaximumCapacity, room.MinimumCapacity, room.Capacity, allGuest.Count + 1);
                        if (isValid.Item1 == false)
                        {
                            isValid.Item2 = "Booking Attendee limit mismatch with max or min limit of the the room";

                            return isValid.Item2;
                        }
                    }
                    else
                    {
                        response = "Room is deleted";

                        return response;
                    }

                    var isConstraintSatisfy = await CheckBookingConstraints(eventEntity.Start, eventEntity.End, eventEntity.RoomId, eventEntity.Host, eventEntity.CreatedBy);

                    if (isConstraintSatisfy.Item1 == false)
                    {
                        return isConstraintSatisfy.Item2;
                    }

                    // Assign event selected room color while creating event
                    var slectedRoom = await _unitOfWork.RoomRepository.GetRoomAsync(eventEntity.RoomId, false);

                    if (userClaim == "admin")
                    {
                        eventEntity.State = "approved";

                        if (slectedRoom != null)
                        {
                            eventEntity.Color = slectedRoom.Color;
                        }
                        else
                        {
                            response = "Selected room already deleted not found";

                            return response;
                        }
                    }

                    events.Add(eventEntity);
                }

                await _unitOfWork.BookingRepository.CreateBookingsAsync(events);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex) 
            {
                response = ex.Message;

                return response;
            }

            response = "success";

            return response;
        }

        public async Task<IList<GetEventDTO>> LoadEventAsync(DateTime start, DateTime end, string? user, string? userClaim)
        {
            var allEvent = await _unitOfWork.BookingRepository.LoadEventAsync(start, end, user, userClaim);
            var eventsDTO = new List<GetEventDTO>();

            for (int i = 0; i < allEvent.Count; i++)
            {
                var scheduleEvent = allEvent[i];
                var eventDTO = new GetEventDTO()
                {
                    Id = scheduleEvent.Id,
                    Name = scheduleEvent.Name,
                    Description = scheduleEvent.Description,
                    Color = scheduleEvent.State == "approved" ? scheduleEvent.Room.Color : scheduleEvent.Color,
                    FontColor = scheduleEvent.Room.FontColor,
                    State = scheduleEvent.State,
                    Start = scheduleEvent.Start,
                    End = scheduleEvent.End,
                    RoomId = scheduleEvent.RoomId,
                    CreatedBy = scheduleEvent.CreatedBy,
                    Host = scheduleEvent.Host,
                    Guests = scheduleEvent.Guests,
                    Room = scheduleEvent.Room,
                };
                eventsDTO.Add(eventDTO);
            }

            return eventsDTO;
        }

        public async Task<IList<GetEventDTO>> LoadGuestEventAsync(DateTime start, DateTime end, string? user, string? userClaim)
        {
            var allEvent = await _unitOfWork.BookingRepository.LoadGuestEventAsync(start, end, user, userClaim);
            var eventsDTO = new List<GetEventDTO>();

            for (int i = 0; i < allEvent.Count; i++)
            {
                var scheduleEvent = allEvent[i];
                var eventDTO = new GetEventDTO()
                {
                    Id = scheduleEvent.Id,
                    Name = scheduleEvent.Name,
                    Description = scheduleEvent.Description,
                    Color = scheduleEvent.State == "approved" ? scheduleEvent.Room.Color : scheduleEvent.Color,
                    State = scheduleEvent.State,
                    FontColor = scheduleEvent.Room.FontColor,
                    Start = scheduleEvent.Start,
                    End = scheduleEvent.End,
                    RoomId = scheduleEvent.RoomId,
                    CreatedBy = scheduleEvent.CreatedBy,
                    Host = scheduleEvent.Host,
                    Guests = scheduleEvent.Guests,
                    Room = scheduleEvent.Room,
                };
                eventsDTO.Add(eventDTO);
            }

            return eventsDTO;
        }

        public async Task<string> DeleteBookingAsync(Guid id)
        {
            string response = string.Empty;

            var eventEntity = await _unitOfWork.BookingRepository.GetBookingAsync(id);

            if (eventEntity == null || eventEntity.CreatedBy is null)
            {
                response = "not found";
                return response;
            }

            await _unitOfWork.BookingRepository.DeleteBookingAsync(eventEntity);
            await _unitOfWork.SaveAsync();

            response = "success";

            return response;
        }


        public async Task<string> EditBookingAsync(EditEventDTO eventDTO, string currentUser, string userClaim)
        {
            string response = string.Empty;

            try
            {
                var existingEvent = await _unitOfWork.BookingRepository.GetEventAsync(eventDTO.Id);
                if (existingEvent != null && existingEvent?.CreatedBy != null)
                {
                    existingEvent.Start = eventDTO.Start;
                    existingEvent.End = eventDTO.End;
                    existingEvent.RoomId = eventDTO.RoomId;

                    // Check the state pending or approved
                    if( existingEvent.State == "approved")
                    {
                        response = "Approved request can't be updated";
                        
                        return response;
                    }

                    // Check is host is the current user or not.
                    if(existingEvent.Host.Trim().Equals(currentUser.Trim()) == false && userClaim == "user")
                    {
                        response = "Can't Update !Current user is not the host user";

                        return response;
                    }

                    // Check meeting booking time limit with room maximum and minimum time.
                    var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
                    var isValid = ValidateEventTimeLimit(existingEvent.Start, existingEvent.End, eventTimeEntity);
                    var allGuest = existingEvent.Guests.Select(x => x.User.Trim()).ToList();

                    if (isValid.Item1 == false)
                    {
                        return isValid.Item2;
                    }

                    // Check booking minimum and maximum room attendee limit.
                    var room = await _unitOfWork.RoomRepository.GetRoomAsync(existingEvent.RoomId, false);

                    if (room != null && room?.Capacity != 0)
                    {
                        isValid = CheckBookingAttendeeLimit(room.MaximumCapacity, room.MinimumCapacity, room.Capacity, allGuest.Count + 1);
                        if (isValid.Item1 == false)
                        {
                            isValid.Item2 = "Booking Attendee limit mismatch with max or min limit of the the room";

                            return isValid.Item2;
                        }
                    }
                    else
                    {
                        response = "Room is deleted";

                        return response;
                    }

                    var isConstraintsSatisfy = await CheckBookingEditConstraints(existingEvent.Start, existingEvent.End, existingEvent.RoomId, existingEvent.Id, existingEvent.CreatedBy, existingEvent.Host);

                    if (isConstraintsSatisfy.Item1 == false)
                    {
                        return isConstraintsSatisfy.Item2;
                    }

                    await _unitOfWork.BookingRepository.EditBookingAsync(existingEvent);
                    await _unitOfWork.SaveAsync();

                    response = "success";
                }
                else
                {
                    response = "Event doesn't exist, May be deleted.";
                }
                

                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                response = ex.Message;
                return response;

            }
            catch (Exception ex)
            {
                response = ex.Message;

                return response;
            }
        }

        public async Task<string> EditBookingByIdAsync(EditEventDTO eventDTO, string currentUser, IList<string> allUser, string userClaim)
        {
            string response = string.Empty;
            try
            {
                var existingEvent = await _unitOfWork.BookingRepository.GetBookingByIdAsync(eventDTO.Id);

                if (existingEvent is null || existingEvent.Count == 0)
                {
                    response = "not found";
                    return response;
                }

                var eventEntity = existingEvent[0];

                eventEntity.Name = eventDTO.Name;
                eventEntity.Start = eventDTO.Start;
                eventEntity.End = eventDTO.End;
                eventEntity.Host = eventDTO.Host;
                eventEntity.State = eventDTO.State;
                eventEntity.Description = eventDTO.Description;
                
                eventEntity.LastUpdatedAtUTC = DateTime.UtcNow;

                if(eventDTO.State == "approved")
                {
                    var oneRoom = await _unitOfWork.RoomRepository.GetRoomAsync(eventDTO.RoomId,false);
                    if (oneRoom != null)
                    { 
                        eventEntity.Color = oneRoom.Color;
                    } 
                }
                else if(eventDTO.State == "pending")
                {
                    eventEntity.Color = "#FFA500";
                }

                var existingGuests = eventEntity.Guests.ToList();

                var newGuestUsers = eventDTO.AllGuest.Split(',').Select(name => name.Trim()).Where(name => !string.IsNullOrEmpty(name)).ToList();

                // Check is host is the current user or not.
                if (eventEntity.Host.Trim().Equals(currentUser.Trim()) == false && userClaim != "admin")
                {
                    response = "Can't Update !Current user is not the host user";

                    return response;
                }

                // Check meeting booking time limit with room maximum and minimum time.
                var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
                var isValid = ValidateEventTimeLimit(eventEntity.Start, eventEntity.End, eventTimeEntity);
                var allGuest = newGuestUsers;

                if (isValid.Item1 == false)
                {
                    return isValid.Item2;
                }

                // Check meeting booking user & guests validity in users list.
                isValid = ValidateMeetingAttendee(allGuest, eventEntity.Host, allUser, eventEntity.CreatedBy, userClaim);

                if (isValid.Item1 == false)
                {
                    return isValid.Item2;
                }


                // Check booking minimum and maximum room attendee limit.
                var room = await _unitOfWork.RoomRepository.GetRoomAsync(eventEntity.RoomId, false);

                if (room != null && room?.Capacity != 0)
                {
                    isValid = CheckBookingAttendeeLimit(room.MaximumCapacity, room.MinimumCapacity, room.Capacity, allGuest.Count + 1);
                    if (isValid.Item1 == false)
                    {
                        isValid.Item2 = "Booking Attendee limit mismatch with max or min limit of the the room";

                        return isValid.Item2;
                    }

                }
                else
                {
                    response = "Room is deleted";

                    return response;
                }

                var isConstraintsSatisfy = await CheckBookingEditConstraints(eventEntity.Start, eventEntity.End, eventEntity.RoomId, eventEntity.Id, eventEntity.CreatedBy, eventEntity.Host);

                if (isConstraintsSatisfy.Item1 == false)
                {
                    return isConstraintsSatisfy.Item2;
                }

                // Change color for accepted request.
                var selectedRoom = await _unitOfWork.RoomRepository.GetRoomAsync(eventDTO.RoomId, false);

                if (eventEntity.State == "approved")
                {
                    
                    if(selectedRoom != null)
                    {
                        eventEntity.Color = selectedRoom.Color;
                    }
                }

                var guestsToRemove = existingGuests.Where(g => !newGuestUsers.Contains(g.User)).ToList();
                var guestsToAdd = newGuestUsers.Where(user => !existingGuests.Any(g => g.User == user))
                    .Select(user => new Guest
                    {
                        Id = Guid.NewGuid(),
                        User = user,
                        EventId = eventDTO.Id,
                        Event = existingGuests[0].Event,
                        CreatedAtUTC = DateTime.UtcNow,
                        LastUpdatedAtUTC = DateTime.UtcNow
                    })
                    .ToList();

                // Explicitly remove guests
                foreach (var guest in guestsToRemove)
                {
                    await _unitOfWork.GuestRepository.RemoveGuestAsync(guest);
                }

                // Explicitly add new guests
                foreach (var newGuest in guestsToAdd)
                {
                    await _unitOfWork.GuestRepository.AddGuestAsync(newGuest);
                }

                // Update LastUpdatedAtUTC for remaining guests
                foreach (var guest in existingGuests.Except(guestsToRemove))
                {
                    guest.LastUpdatedAtUTC = DateTime.UtcNow;
                }

                // Save changes
                await _unitOfWork.SaveAsync();

                response = "success";
                return response;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                response = ex.Message;

                return response;
            }
            catch (Exception ex)
            {
                response = ex.Message;

                return response;
            }
        }

        public async Task<GetEventDTO> GetEventByIdAsync(Guid id)
        {
            var eventEntity = await _unitOfWork.BookingRepository.GetBookingByIdAsync(id);

            if (eventEntity == null || eventEntity.Count == 0)
            {
                return null;
            }

            var eventDTO = new GetEventDTO
            {
                Id = eventEntity[0].Id,
                Name = eventEntity[0].Name,
                Start = eventEntity[0].Start,
                End = eventEntity[0].End,
                Color = eventEntity[0].Color,
                Description = eventEntity[0].Description,
                RoomId = eventEntity[0].RoomId,
                Guests = eventEntity[0].Guests,
                CreatedBy = eventEntity[0].CreatedBy,
                Host = eventEntity[0].Host,
                State = eventEntity[0].State
            };

            return eventDTO;
        }
    }
}