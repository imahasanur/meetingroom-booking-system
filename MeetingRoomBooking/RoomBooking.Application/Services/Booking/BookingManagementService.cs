using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomBooking.Application.Domain.Entities;
using RoomBooking.Application.DTO;
using System;
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
            
            if(maximumCapacity == null && minimumCapacity == null && (members > capacity))
            {
                isValid = false;
            }
            else if(maximumCapacity != null && minimumCapacity == null && (!(members  <= maximumCapacity && members <= capacity)))
            {
                isValid = false;
                
            }
            else if(maximumCapacity == null && minimumCapacity != null && (members < minimumCapacity && members <= capacity))
            {
                isValid = false;
            }
            else if(members < minimumCapacity || (members > maximumCapacity ) || members > capacity)
            {
                isValid = false;
            }

            return (isValid, response);
        }

        (bool,string) ValidateEventTimeLimit(DateTime start, DateTime end, IList<RoomBooking.Application.Domain.Entities.EventTime> eventTimeEntity)
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

        (bool,string) ValidateMeetingAttendee(IList<string> guests, string host, IList<string> users, string bookingMaker)
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

            if (isValid == true) 
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

            return (isValid, response);
        }

        public async Task<string> CreateBookingAsync(CreateEventDTO eventDTO, IList<string> allUser)
        {
            string response = string.Empty;

            var eventEntity = new RoomBooking.Application.Domain.Entities.Event()
            {
                Id = eventDTO.Id,
                Name = eventDTO.Name,
                Color = eventDTO.Color,
                State = eventDTO.State,
                Start = eventDTO.Start,
                End = eventDTO.End,
                CreatedAtUTC = eventDTO.CreatedAtUTC,
                CreatedBy = eventDTO.CreatedBy,
                Host = eventDTO.Host,
                RoomId = eventDTO.RoomId,
                Guests = eventDTO.Guests,
            };

            // Check meeting booking time limit with room maximum and minimum time.
            var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
            var isValid = ValidateEventTimeLimit(eventEntity.Start, eventEntity.End, eventTimeEntity);
            var allGuest = eventEntity.Guests.Select(x => x.User.Trim()).ToList();

            if (isValid.Item1 == false)
            {
                return isValid.Item2;
            }

            // Check meeting booking user & guests validity in users list.
            isValid = ValidateMeetingAttendee(allGuest, eventEntity.Host, allUser, eventEntity.CreatedBy);

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

            // Check pending request for request maker.
            var bookings = await _unitOfWork.BookingRepository.GetBookingByMakerAsync(eventEntity.CreatedBy);
            if(bookings != null && bookings.Count > 0)
            {
                response = "Can't do multiple pending meeting booking request";

                return response;
            }

            // Check user for same room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckBookingOverlapping(eventEntity.Start, eventEntity.End, eventEntity.RoomId);
            if(bookings != null && bookings.Count > 0)
            {
                response = "Found same room , same day overlapping meeting.";

                return response;
            }

            // Check user for different room , same day overlapping meeting.
            bookings = await _unitOfWork.BookingRepository.CheckAnyRoomBookingOverlappingByUser(eventEntity.Start, eventEntity.End, eventEntity.CreatedBy);
            if (bookings != null && bookings.Count > 0)
            {
                response = "Found different room , same day overlapping meeting by a booking creator.";

                return response;
            }

            // Check event start time is backward or not.
            var isBackward = eventEntity.Start > DateTime.Now ? true: false;
            if(isBackward == false)
            {
                response = "Event start time can't be set backward than current time while creatig";

                return response;
            }

            await _unitOfWork.BookingRepository.CreateBookingAsync(eventEntity);
            await _unitOfWork.SaveAsync();

            response = "success";

            return response;
        }

        public async Task<IList<GetEventDTO>> GetAllEventAsync(DateTime start, DateTime end, string? user)
        {
            var allEvent = await _unitOfWork.BookingRepository.GetAllEventAsync(start, end, user);
            var eventsDTO = new List<GetEventDTO>();

            for (int i = 0; i < allEvent.Count; i++)
            {
                var scheduleEvent = allEvent[i];
                var eventDTO = new GetEventDTO()
                {
                    Id = scheduleEvent.Id,
                    Name = scheduleEvent.Name,
                    Color = scheduleEvent.Color,
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


        public async Task<string> EditBookingAsync(EditEventDTO eventDTO, string currentUser)
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


                    // Check user for same room , same day overlapping meeting.
                    var bookings = await _unitOfWork.BookingRepository.CheckEditBookingOverlapping(existingEvent.Start, existingEvent.End, existingEvent.RoomId, existingEvent.Id);
                    if (bookings != null && bookings.Count > 0)
                    {
                        response = "Found same room , same day overlapping meeting.";

                        return response;
                    }

                    // Check user for different room , same day overlapping meeting.
                    bookings = await _unitOfWork.BookingRepository.CheckEditAnyRoomBookingOverlappingByUser(existingEvent.Start, existingEvent.End, existingEvent.CreatedBy, existingEvent.Id);
                    if (bookings != null && bookings.Count > 0)
                    {
                        response = "Found different room , same day overlapping meeting by a booking creator.";

                        return response;
                    }

                    // Check event start time is backward or not.
                    var isBackward = existingEvent.Start > DateTime.Now ? true : false;
                    if (isBackward == false)
                    {
                        response = "Event start time can't be set backward than current time while updating";

                        return response;
                    }

                    // Check user in meeting or not
                    var isInMeeting = allGuest.Contains(currentUser);

                    if (isInMeeting == false) 
                    {
                        if (!(currentUser.Trim().Equals(existingEvent.Host.Trim()))) 
                        {
                            response = "Current user are not in meeting, Can't update";
                            return response;
                        }
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

        public async Task<string> EditBookingByIdAsync(EditEventDTO eventDTO, string currentUser, IList<string> allUser)
        {
            string response = string.Empty;
            try
            {
                var existingEvent = await _unitOfWork.BookingRepository.GetBookingByIdAsync(eventDTO.Id);

                if (existingEvent.Count == 0)
                {
                    response = "not found";
                    return response;
                }

                var eventEntity = existingEvent[0];
                eventEntity.Name = eventDTO.Name;
                eventEntity.Start = eventDTO.Start;
                eventEntity.End = eventDTO.End;
                eventEntity.Host = eventDTO.Host;

                var existingGuests = eventEntity.Guests.ToList();

                var newGuestUsers = eventDTO.AllGuest.Split(',').Select(name => name.Trim()).Where(name => !string.IsNullOrEmpty(name)).ToList();


                // Check meeting booking time limit with room maximum and minimum time.
                var eventTimeEntity = await _unitOfWork.EventTimeRepository.GetTimeLimitAsync();
                var isValid = ValidateEventTimeLimit(eventEntity.Start, eventEntity.End, eventTimeEntity);
                var allGuest = newGuestUsers;

                if (isValid.Item1 == false)
                {
                    return isValid.Item2;
                }

                // Check meeting booking user & guests validity in users list.
                isValid = ValidateMeetingAttendee(allGuest, eventEntity.Host, allUser, eventEntity.CreatedBy);

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


                // Check user for same room , same day overlapping meeting.
                var bookings = await _unitOfWork.BookingRepository.CheckEditBookingOverlapping(eventEntity.Start, eventEntity.End, eventEntity.RoomId, eventEntity.Id);
                if (bookings != null && bookings.Count > 0)
                {
                    response = "Found same room , same day overlapping meeting.";

                    return response;
                }

                // Check user for different room , same day overlapping meeting.
                bookings = await _unitOfWork.BookingRepository.CheckEditAnyRoomBookingOverlappingByUser(eventEntity.Start, eventEntity.End, eventEntity.CreatedBy, eventEntity.Id);
                if (bookings != null && bookings.Count > 0)
                {
                    response = "Found different room , same day overlapping meeting by a booking creator.";

                    return response;
                }

                // Check event start time is backward or not.
                var isBackward = eventEntity.Start > DateTime.Now ? true : false;
                if (isBackward == false)
                {
                    response = "Event start time can't be set backward than current time while updating";

                    return response;
                }

                // Check user in meeting or not
                var isInMeeting = allGuest.Contains(currentUser);

                if (isInMeeting == false)
                {
                    if (!(currentUser.Trim().Equals(eventEntity.Host.Trim())))
                    {
                        response = "Current user are not in meeting, Can't update";
                        return response;
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