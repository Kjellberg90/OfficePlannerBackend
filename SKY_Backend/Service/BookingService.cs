﻿using DAL;
using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IRoomAccess _roomAccess;

        public BookingService(IBookingAccess bookingService, IRoomAccess roomAccess)
        {
            _bookingAccess = bookingService;
            _roomAccess = roomAccess;
        }

        public IEnumerable<Booking> GetBookings()
        {
            return _bookingAccess.ReadBookingsData();
        }

        public void PostBookings()
        {
            _bookingAccess.PrintGroupToFile();
        }

        public void PostSingleBooking(SingleBookingDTO singleBookingDTO)
        {
            var bookedDate = DateTime.Parse(singleBookingDTO.Date);
            var room = _roomAccess.ReadRoomsData()
                .Where(r => r.ID == singleBookingDTO.RoomId)
                .FirstOrDefault();

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            var singleBooking = new SingleBooking()
            {
                Id = singleBookingDTO.RoomId,
                Date = bookedDate,
                Name = singleBookingDTO.Name,
                BookedRoom = room
            };

            _bookingAccess.PostSingleBooking(singleBooking);
        }
    }
}
