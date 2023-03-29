﻿using DAL;
using DAL.Models;
using DAL.SQLModels;
using Service.DateHandler;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IDateHandler _dateHandler;

        public RoomService(IDateHandler dateHandler)
        {
            _dateHandler = dateHandler;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date)
        {
            var dayNr = _dateHandler.ConvertDateToDaySequence(date);
            var roomInfoList = new List<RoomInfoDTO>();
            var currentDate = DateTime.Parse(date);

            using (var context = new SkyDbContext())
            {
                var rooms = context.Rooms.ToList();
                var bookings = context.Bookings
                    .Where(b => b.DayNr == dayNr)
                    .ToList();

                var singleRoomBookings = context.SingleRoomBookings
                    .Where(s => s.DayNr == dayNr)
                    .ToList();

                foreach (var room in rooms)
                {
                    var booking = bookings.Find(b => b.RoomID == room.Id);
                    var singleRoomBooking = singleRoomBookings.Find(b => b.RoomID == room.Id);

                    SQLGroup? group = null;

                    if (booking != null)
                    {
                        group = context.Groups.FirstOrDefault(g => g.Id == booking.GroupID);
                    }
                    else if (singleRoomBooking != null)
                    {
                        group = context.Groups.FirstOrDefault(g => g.Id == singleRoomBooking.GroupID);
                    }

                    var groupSize = group == null ? 0 : group.GroupSize;

                    var availableSeats = GetAvailableSeats(room, groupSize, date);

                    var singelBookings = context.SingleBookings
                        .Where(s => s.Date == currentDate && s.RoomID == room.Id)
                        .ToList()
                        .Count();

                    roomInfoList.Add(new RoomInfoDTO
                    {
                        Name = room.Name,
                        RoomId = room.Id,
                        Seats = room.Seats,
                        GroupName = group == null ? "" : group.Name,
                        AvailableSeats = availableSeats >= 0 ? availableSeats : 0,
                        SingleBookings = singelBookings
                    });

                }

                return roomInfoList;
            }
        }

        public int GetAvailableSeats(SQLRoom room, int groupSize, string date)
        {
            using (var context = new SkyDbContext())
            {
                var singleBookings = context.SingleBookings
                    .Where(s => s.Date == DateTime.Parse(date) && s.RoomID == room.Id);

                var availableSeats = room.Seats - singleBookings.Count() - groupSize;
                return availableSeats;
            }
        }
    }
}
