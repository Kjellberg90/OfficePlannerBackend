using DAL;
using DAL.Models;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        private readonly IDateConverter _dateConverter;

        public RoomService(IDateConverter dateConverter)
        {
            _dateConverter = dateConverter;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var roomInfoList = new List<RoomInfoDTO>();

            using (var context = new SkyDbContext())
            {
                var rooms = context.Rooms.ToList();
                var bookings = context.Bookings
                    .Where(b => b.DayNr == dayNr)
                    .ToList();

                foreach (var room in rooms)
                {
                    var booking = bookings.Find(b => b.RoomID == room.Id);

                    SQLGroup? group = null;
                    
                    if (booking != null)
                    {
                        group = context.Groups.FirstOrDefault(g => g.Id == booking.GroupID);
                    }

                    var groupSize = group == null ? 0 : group.GroupSize;

                    var availableSeats = GetAvailableSeats(room, groupSize, date);

                    roomInfoList.Add(new RoomInfoDTO
                    {
                        Name = room.Name,
                        RoomId = room.Id,
                        Seats = room.Seats,
                        GroupName = group == null ? "" : group.Name,
                        AvailableSeats = availableSeats >= 0 ? availableSeats : 0
                    }); ;

                }

                return roomInfoList;
            }
        }

        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(string date)
        {
            var formattedDate = _dateConverter.ConvertDateToDaySequence(date);
            var scheduleWeek = GetScheduleWeekNr(formattedDate);
            var weekDays = GetWeekDays(scheduleWeek, formattedDate);

            using (var context = new SkyDbContext())
            {
                var overviewList = new List<AdminRoomOverviewDTO>();
                var rooms = context.Rooms.ToList();

                foreach (var room in rooms)
                {
                    var overview = GetOverview(room, weekDays);
                    overviewList.Add(overview);
                }

                return overviewList;
            }
        }

        public AdminRoomOverviewDTO GetOverview(SQLRoom room, List<int> days)
        {
            using (var context = new SkyDbContext())
            {
                var groupNamesList = new List<string>();

                foreach (var day in days)
                {
                    var booking = context.Bookings
                        .Where(b => b.DayNr == day && b.RoomID == room.Id)
                        .FirstOrDefault();

                    if (booking != null)
                    {
                        groupNamesList.Add(context.Groups.FirstOrDefault(g => g.Id == booking.GroupID).Name);
                    }
                    else
                    {
                        groupNamesList.Add("");
                    }
                }

                return new AdminRoomOverviewDTO { roomName = room.Name, groupNames = groupNamesList };
            }
        }

        public IEnumerable<AdminRoomDTO> AdminGetRooms()
        {
            using (var context = new SkyDbContext())
            {
                var rooms = context.Rooms.ToList();
                var adminRoomList = new List<AdminRoomDTO>();

                foreach (var room in rooms)
                {
                    adminRoomList.Add(new AdminRoomDTO
                    {
                        Id = room.Id,
                        Name = room.Name,
                        Seats = room.Seats,
                    });
                }
                return adminRoomList;
            }
        }

        public void AdminPostRoom(AdminPostRoomDTO adminAddRoom)
        {
            using (var context = new SkyDbContext())
            {
                context.Rooms.Add(new SQLRoom
                {
                    Name = adminAddRoom.Name,
                    Seats = adminAddRoom.Seats
                });

                context.SaveChanges();
            }
        }

        public void AdminDeleteRoom(AdminDeleteRoomDTO adminDeleteRoom)
        {
            using (var context = new SkyDbContext())
            {
                var room = context.Rooms.FirstOrDefault(r => r.Id == adminDeleteRoom.Id);
                if (room == null) throw new ArgumentNullException(nameof(room));

                context.Rooms.Remove(room);
                context.SaveChanges();
            }
        }

        public void UpdateRoom(int roomId, AdminEditRoomDTO adminEditRoom)
        {
            using (var context = new SkyDbContext())
            {
                var room = context.Rooms.FirstOrDefault(r => r.Id == roomId);
                if (room == null) throw new ArgumentNullException($"Room {roomId}");

                room.Name = adminEditRoom.Name;
                room.Seats = adminEditRoom.Seats;

                context.SaveChanges();
            }
        }

        public int GetAvailableSeats(SQLRoom room, int groupSize, string date)
        {
            using (var context = new SkyDbContext())
            {
                var singleBookings = context.SingleBookings
                    .Where(s => (s.Date == DateTime.Parse(date)) && (s.RoomID == room.Id));

                var availableSeats = room.Seats - singleBookings.Count() - groupSize;
                return availableSeats;
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

        public List<int> GetWeekDays(int week, int dayNr)
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
