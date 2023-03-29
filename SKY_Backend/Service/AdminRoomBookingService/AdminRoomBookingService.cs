using DAL;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminRoomBookingService
{
    public class AdminRoomBookingService : IAdminRoomBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IDateConverter _dateConverter;

        public AdminRoomBookingService(IBookingAccess bookingAcess, IDateConverter dateConverter)
        {
            _bookingAccess = bookingAcess;
            _dateConverter = dateConverter;
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

        public List<GroupBookedToRoom> GetBookingsForRoom()
        {
            using (var context = new SkyDbContext())
            {
                var GroupsBookedToRooms = context.SingleRoomBookings.OrderByDescending(b => b.Date).ToList();
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

        public void DeleteOldSingleRoomBookings()
        {
            using (var context = new SkyDbContext())
            {
                var singleRoomBookings = context.SingleRoomBookings.ToList();

                var oldSingleRoomBookings = singleRoomBookings
                    .Where(d => d.Date < DateTime.Now.AddDays(-1))
                    .ToList();

                if (oldSingleRoomBookings == null) throw new Exception("No old bookings where found");

                foreach (var booking in oldSingleRoomBookings)
                {
                    context.SingleRoomBookings.Remove(booking);
                }
                context.SaveChanges();
            }
        }

        public void EditGroupToRoomBooking(int bookingId, GroupToRoomBookingDTO groupToRoomBooking)
        {
            using (var context = new SkyDbContext())
            {
                var regularBookings = context.Bookings.ToList();
                var date = DateTime.Parse(groupToRoomBooking.Date);
                var dayNr = _dateConverter.ConvertDateToDaySequence(groupToRoomBooking.Date);

                var booking = context.SingleRoomBookings.
                    Where(b => b.Id == bookingId).
                    FirstOrDefault();

                var rooms = context.Rooms.ToList();

                var room = rooms
                    .Where(r => r.Id == groupToRoomBooking.RoomId)
                    .FirstOrDefault();

                var groups = context.Groups.ToList();

                var group = groups
                    .Where(g => g.Id == groupToRoomBooking.GroupId)
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

                var bookingsByDayNr = regularBookings
                    .Where(b => b.DayNr == dayNr)
                    .ToList();

                var isBooked = false;
                foreach (var item in bookingsByDayNr)
                {
                    var regularBooking = regularBookings
                        .Where(b => b.RoomID == item.RoomID && b.DayNr == item.DayNr)
                        .FirstOrDefault();

                    if (regularBooking.RoomID == groupToRoomBooking.RoomId && regularBooking.DayNr == dayNr)
                    {
                        isBooked = true;
                    }
                }

                if (booking != null && isBooked != true && availableSeats)
                {
                    booking.RoomID = groupToRoomBooking.RoomId;
                    booking.GroupID = groupToRoomBooking.GroupId;
                    booking.DayNr = dayNr;
                    booking.Date = date;
                }
                else
                {
                    throw new Exception("Room is already booked by another group");
                }

                context.SaveChanges();
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
            var firstWeekDay = 7 * (week - 1) + 1;

            for (int i = firstWeekDay; i < firstWeekDay + 5; i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}
