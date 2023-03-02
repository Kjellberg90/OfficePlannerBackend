using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using DAL.SQLModels;

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IDateConverter _dateConverter;
        private readonly IBookingAccess _bookingAccess;
        public GroupService(IDateConverter dateConverter, IBookingAccess bookingAccess)
        {
            this._dateConverter = dateConverter;
            this._bookingAccess = bookingAccess;
        }

        public GroupInfoDTO GetGroupInfo(string date, int groupId)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);

            using (var context = new SkyDbContext())
            {
                var group = context.Groups.FirstOrDefault(g => g.Id == groupId);

                var booking = context.Bookings
                    .Where(b => b.GroupID == groupId && b.DayNr == dayNr)
                    .FirstOrDefault()
                    ;

                GroupInfoDTO groupInfo;

                if (booking == null)
                {
                    groupInfo = new GroupInfoDTO { Name = group.Name, GroupSize = group.GroupSize };
                } else
                {
                    var room = context.Rooms.FirstOrDefault(r => r.Id == booking.RoomID);
                    groupInfo = new GroupInfoDTO { Name = group.Name, GroupSize = group.GroupSize, BookedRoom = room };
                }

                return groupInfo;
            }
        }

        public IEnumerable<SQLGroup> GetGroups()
        {
            using (var context = new SkyDbContext())
            {
                var groupList = context.Groups.ToList();
                return groupList;
            }
        }

        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup)
        {
            using (var context = new SkyDbContext())
            {
                var group = context.Groups
                    .Where(g => g.Id == groupId)
                    .FirstOrDefault();

                if (group == null) throw new Exception("Group not found");

                group.Name = newGroup.Name;
                group.GroupSize = newGroup.GroupSize;
                if (newGroup.Division != null) group.Department = newGroup.Division;

                context.SaveChanges();
            }
        }

        public void DeleteGroup(int groupId)
        {
            using (var context = new SkyDbContext())
            {
                var group = context.Groups.FirstOrDefault(r => r.Id == groupId);
                if (group == null) throw new ArgumentNullException(nameof(group));

                context.Groups.Remove(group);
                context.SaveChanges();
            }
        }

        public void AddGroup(AddGroupDTO addGroupDTO)
        {
            using (var context = new SkyDbContext())
            {
                context.Groups.Add(new SQLGroup
                {
                    Name = addGroupDTO.Name,
                    GroupSize = addGroupDTO.GroupSize,
                    Department = addGroupDTO.Division
                });

                context.SaveChanges();
            }
        }

        public List<WeeklyGroupScheduleDTO> GetWeeklysSchedule(string date, int groupId)
        {
            var formattedDate = DateTime.Parse(date);
            var formattedDateToString = formattedDate.DayOfWeek.ToString();
            var monday = new DateTime();

            if (formattedDateToString == "Sunday")
            {
                monday = formattedDate.AddDays(-(int)formattedDate.DayOfWeek + (int)DayOfWeek.Monday - 7);
            }
            else
            {
                monday = formattedDate.AddDays(-(int)formattedDate.DayOfWeek + (int)DayOfWeek.Monday);
            }

            List<string> weeksDates = new List<string>();

            var dayNumber = _dateConverter.ConvertDateToDaySequence(date);
            var scheduleWeek = GetScheduleWeekNr(dayNumber);
            var weekDays = GetWeekDays(scheduleWeek, dayNumber);

            for (int i = 0; i < 7; i++)
            {
                var individualDate = monday.AddDays(i);
                var dateToFormattedString = individualDate.ToString("dddd");
                weeksDates.Add(dateToFormattedString);
            }

            List<WeeklyGroupScheduleDTO> weeklyRoomSchedule = new List<WeeklyGroupScheduleDTO>();

            using (var context = new SkyDbContext())
            {
                for (int i = weekDays.Min(); i < (weekDays.Min() + weekDays.Count); i++)
                {
                    var bookings = context.Bookings
                        .Where(b => b.GroupID == groupId && b.DayNr == i)
                        .FirstOrDefault();

                    string roomName = "Unbooked";
                    if (bookings != null)
                    {
                        roomName = context.Rooms.FirstOrDefault(r => r.Id == bookings.RoomID).Name;
                    }                        

                    weeklyRoomSchedule.Add(new WeeklyGroupScheduleDTO
                    {
                        date = weeksDates[i - weekDays.Min()],
                        room = roomName,
                    });
                }
                return weeklyRoomSchedule;
            }
        }

        public GetCurrentWeekDTO GetCurrentWeekAndDay(string date)
        {
            var formattedDate = DateTime.Parse(date);
            var formattedDateToString = formattedDate.DayOfWeek.ToString();
            var dateInputday = DateTime.Parse(date);
            var dayOfWeek = dateInputday.DayOfWeek.ToString();
            var monday = new DateTime();

            if (formattedDateToString == "Sunday")
            {
                monday = formattedDate.AddDays(-(int)formattedDate.DayOfWeek + (int)DayOfWeek.Monday - 7);
            }
            else
            {
                monday = formattedDate.AddDays(-(int)formattedDate.DayOfWeek + (int)DayOfWeek.Monday);
            }

            Calendar cal = new CultureInfo("sv-SE").Calendar;

            int week = cal.GetWeekOfYear(monday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var currentWeek = new GetCurrentWeekDTO()
            {
                Week = week,
                Day = dayOfWeek,
            };

            return currentWeek;
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

            for (int i = firstWeekDay; i < (firstWeekDay + 7); i++)
            {
                list.Add(i);
            }

            return list;
        }
    }

}
