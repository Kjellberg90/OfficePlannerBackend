using DAL;
using DAL.Models;
using DAL.SQLModels;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IDateConverter _dateConverter;

        public BookingService(IBookingAccess bookingAcess, IDateConverter dateConverter)
        {
            _bookingAccess = bookingAcess;
            _dateConverter = dateConverter;
        }

        public IEnumerable<SQLBooking> GetBookings()
        {
            using (var context = new SkyDbContext())
            {
                var bookings = context.Bookings.ToList();
                return bookings;
            }

        }

        public void PostBookings()
        {
            _bookingAccess.PrintGroupToFile();
        }

        public IEnumerable<UserDTO> GetSingleBookingsForDate(string date, int bookedRoomId)
        {
            using (var context = new SkyDbContext())
            {
                var singleBookings = context.SingleBookings.ToList();
                var currentDate = DateTime.Parse(date);
                var singleBookingsOnDay = singleBookings.Where(x => x.Date == currentDate && x.RoomID == bookedRoomId);

                var users = new List<UserDTO>();

                foreach (var user in singleBookingsOnDay)
                {
                    users.Add(new UserDTO
                    {
                        Id = user.ID,
                        UserName = user.Name
                    });
                }

                return users;
            }
        }

        public void PostSingleBooking(SingleBookingDTO singleBookingDTO)
        {
            using (var context = new SkyDbContext())
            {
                var bookedDate = DateTime.Parse(singleBookingDTO.Date);
                var room = context.Rooms
                    .Where(r => r.Id == singleBookingDTO.RoomId)
                    .FirstOrDefault();

                if (room == null) throw new Exception("Room not found");

                context.SingleBookings.Add(new SQLSingleBooking
                {
                    Name = singleBookingDTO.Name,
                    Date = bookedDate,
                    RoomID = singleBookingDTO.RoomId
                });

                context.SaveChanges();
            }
        }

        public void DeleteSingleBooking(DeleteSingleBookingDTO deleteSingleBooking)
        {
            using (var context = new SkyDbContext())
            {
                var date = DateTime.Parse(deleteSingleBooking.date);
                var singleBookingToDelete = context.SingleBookings
                    .Where(x => x.Name == deleteSingleBooking.userName && x.Date == date && x.RoomID == deleteSingleBooking.roomId)
                    .FirstOrDefault();

                if (singleBookingToDelete == null) throw new Exception("SingleBooking not found");

                context.SingleBookings.Remove(singleBookingToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateBookings(UpdateBookingsDTO[] updateBookings, string date)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var weekNr = GetScheduleWeekNr(dayNr);
            var weekDays = GetWeekDays(weekNr);

            foreach (var booking in updateBookings)
            {
                PostUpdates(booking, weekDays);
            }
        }

        public void PostUpdates(UpdateBookingsDTO updateInfo, List<int> days)
        {
            using (var context = new SkyDbContext())
            {
                var roomId = context.Rooms.FirstOrDefault(r => r.Name == updateInfo.roomName).Id;

                foreach (var day in days.Select((value, i) => new { i, value }))
                {
                    var booking = context.Bookings
                        .Where(b => b.DayNr == day.value && b.RoomID == roomId)
                        .FirstOrDefault();

                    if (updateInfo.groupNames[day.i] == "" && booking != null)
                    {
                        context.Bookings.Remove(booking);
                        context.SaveChanges();
                        continue;
                    }
                    else if (updateInfo.groupNames[day.i] != "" && booking == null)
                    {
                        var groupId = context.Groups.FirstOrDefault(g => g.Name == updateInfo.groupNames[day.i]).Id;

                        context.Bookings.Add(new SQLBooking
                        {
                            DayNr = day.value,
                            RoomID = roomId,
                            GroupID = groupId,
                            ScheduleID = 1,
                        });
                        context.SaveChanges();
                    }
                    else if (updateInfo.groupNames[day.i] != "" && booking != null)
                    {
                        var groupId = context.Groups.FirstOrDefault(g => g.Name == updateInfo.groupNames[day.i]).Id;

                        booking.GroupID = groupId;
                        context.SaveChanges();
                    }
                }
            }
        }

        public void PostGroupToRoomBooking(GroupToRoomBookingDTO postGroupToRoomDTO)
        {

            using (var context = new SkyDbContext())
            {
                var regularBookings = context.Bookings.ToList();
                var date = DateTime.Parse(postGroupToRoomDTO.Date);
                var dayNr = _dateConverter.ConvertDateToDaySequence(postGroupToRoomDTO.Date);

                var booking = context.SingleRoomBookings
                    .Where(b => b.RoomID == postGroupToRoomDTO.RoomId && b.Date == date)
                    .FirstOrDefault();

                var bookingsByDayNr = regularBookings
                    .Where(b => b.DayNr == dayNr)
                    .ToList();
                var rooms = context.Rooms.ToList();

                var room = rooms
                    .Where(r => r.Id == postGroupToRoomDTO?.RoomId)
                    .FirstOrDefault();

                var groups = context.Groups.ToList();

                var group = groups
                    .Where(g => g.Id == postGroupToRoomDTO?.GroupId)
                    .FirstOrDefault();

                bool availableSeats = false;
                if (room.Seats >= group.GroupSize)
                {
                    availableSeats = true;
                }
                else
                {
                    throw new Exception("Groupsize is larger then available seats");
                }

                var isBooked = false;
                foreach (var item in bookingsByDayNr)
                {
                    var regularBooking = regularBookings
                        .Where(b => b.RoomID == item.RoomID && b.DayNr == item.DayNr)
                        .FirstOrDefault();

                    if (regularBooking.RoomID == postGroupToRoomDTO.RoomId && regularBooking.DayNr == dayNr)
                    {
                        isBooked = true;
                    }
                }


                if (booking == null && isBooked != true && availableSeats)
                {
                    context.SingleRoomBookings.Add(
                            new SQLSingleRoomBooking
                            {
                                RoomID = postGroupToRoomDTO.RoomId,
                                GroupID = postGroupToRoomDTO.GroupId,
                                Date = date,
                                DayNr = dayNr,
                            }
                        );
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Room is already booked by another group");
                }
            }
        }

        public void DeleteGroupToRoomBooking(int bookingId)
        {
            using (var context = new SkyDbContext())
            {
                var singleRoomBookings = context.SingleRoomBookings.ToList();

                var singleroomBooking = singleRoomBookings
                    .Where(s => s.Id == bookingId)
                    .FirstOrDefault();

                if (singleroomBooking == null) throw new Exception("Booking not found");

                context.SingleRoomBookings.Remove(singleroomBooking);
                context.SaveChanges();
            }
        }

        public List<GroupBookedToRoom> GetBookingsForRoom()
        {
            using (var context = new SkyDbContext())
            {
                var GroupsBookedToRooms = context.SingleRoomBookings.ToList();
                var rooms = context.Rooms.ToList();
                var groups = context.Groups.ToList();

                var roomBookings = new List<GroupBookedToRoom>();

                foreach (var booking in GroupsBookedToRooms)
                {
                    var room = rooms.Where(r => r.Id == booking.RoomID).FirstOrDefault();
                    var group = groups.Where(g => g.Id == booking.GroupID).FirstOrDefault();
                    var date = booking.Date.ToString("yyyy-MM-dd", new CultureInfo("en-GB"));
                    var id = booking.Id;

                    roomBookings.Add(new GroupBookedToRoom
                    {
                        Id = id,
                        RoomName = room.Name,
                        GroupName = group.Name,
                        Date = date,
                    }
                    );
                }
                return roomBookings;
            }
        }

        public void RefreshBookings()
        {
            using (var context = new SkyDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var data = _bookingAccess.ReadBookingsData();

                foreach (var booking in data)
                {
                    var roomsList = booking.Rooms;

                    foreach (var room in roomsList)
                    {
                        if (room.BookedBy != null)
                        {
                            context.Bookings.Add(
                            new SQLBooking
                            {
                                DayNr = booking.DayNr,
                                GroupID = (int)room.BookedBy,
                                RoomID = context.Rooms.Where(x => x.Name == room.Name).FirstOrDefault().Id,
                                ScheduleID = 1
                            }
                            );
                        }
                    }
                }

                context.SaveChanges();


                var rooms = context.Rooms.ToArray();
                var groups = context.Groups.ToArray();
                var schedules = context.Schedules.ToArray();
                var bookings = context.Bookings.ToArray();
                var users = context.Users.ToArray();

                foreach (var user in users)
                {
                    Console.WriteLine("Users: " + user.UserName);
                }
                //foreach (var group in groups)
                //{
                //    Console.WriteLine("Group: " + group.Name);
                //}
                //foreach (var room in rooms)
                //{
                //    Console.WriteLine("Room: " + room.Name);
                //}
                //foreach (var schedule in schedules)
                //{
                //    Console.WriteLine("Schedule: " + schedule.Name);
                //}
                //foreach (var booking in bookings)
                //{
                //    Console.WriteLine("Bookings ID: " + booking.Id);
                //    Console.WriteLine("Bookings DayNr: " + booking.DayNr);
                //    Console.WriteLine("Bookings: RoomId" + booking.RoomID);

                //}
            }
        }

        public int GetScheduleWeekNr(int dayNr)
        {
            if (dayNr == 0) { throw new Exception("Incorrect day number"); }

            if (dayNr >= 1 && dayNr < 8)
            {
                return 1;
            }
            else if (dayNr >= 8 && dayNr < 15)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public List<int> GetWeekDays(int week)
        {
            var list = new List<int>();
            var firstWeekDay = (7 * (week - 1) + 1);

            for (int i = firstWeekDay; i < (firstWeekDay + 5); i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}