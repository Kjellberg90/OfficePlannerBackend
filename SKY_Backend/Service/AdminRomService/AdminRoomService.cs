using DAL;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminRomService
{
    public class AdminRoomService : IAdminRoomService
    {
        private readonly IDateConverter _dateConverter;

        public AdminRoomService(IDateConverter dateConverter)
        {
            _dateConverter = dateConverter;
        }

        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(int weekNr, int scheduleId)
        {
            using (var context = new SkyDbContext())
            {
                var weeks = context.Schedules
                    .Where(s => s.Id == scheduleId)
                    .First().WeekInterval;
                var weekDays = _dateConverter.GetWeekDays(weekNr);
                var overviewList = new List<AdminRoomOverviewDTO>();

                var rooms = context.Rooms.ToList();

                foreach (var room in rooms)
                {
                    var overview = GetOverview(room, weekDays, scheduleId);
                    overviewList.Add(overview);
                }

                return overviewList;
            }
        }

        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(string date, int scheduleId)
        {
            using (var context = new SkyDbContext())
            {
                var dayNr = _dateConverter.ConvertDateToDaySequence(date, 3);
                var weekNr = _dateConverter.GetScheduleWeekNr(dayNr);
                var weeks = context.Schedules
                    .Where(s => s.Id == scheduleId)
                    .First().WeekInterval;
                var weekDays = _dateConverter.GetWeekDays(weekNr);
                var overviewList = new List<AdminRoomOverviewDTO>();

                var rooms = context.Rooms.ToList();

                foreach (var room in rooms)
                {
                    var overview = GetOverview(room, weekDays, scheduleId);
                    overviewList.Add(overview);
                }

                return overviewList;
            }
        }

        public AdminRoomOverviewDTO GetOverview(SQLRoom room, List<int> days, int scheduleId)
        {
            using (var context = new SkyDbContext())
            {
                var groupNamesList = new List<string>();

                foreach (var day in days)
                {
                    var booking = context.Bookings
                        .Where(b => b.DayNr == day && b.RoomID == room.Id && b.ScheduleID == scheduleId)
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
    }
}
