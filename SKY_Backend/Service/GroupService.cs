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

            using (var context = new SkyDbContext())
            {
                var weeks = context.Schedules
                    .Where(s => s.Id == 1)
                    .First().WeekInterval;
                var dayNr = _dateConverter.ConvertDateToDaySequence(date, weeks);
                var group = context.Groups.FirstOrDefault(g => g.Id == groupId);

                var booking = context.Bookings
                    .Where(b => b.GroupID == groupId && b.DayNr == dayNr)
                    .FirstOrDefault()
                    ;

                var singleRoomBookings = context.SingleRoomBookings
                        .Where(s => s.GroupID == groupId && s.DayNr == dayNr)
                        .FirstOrDefault();

                GroupInfoDTO groupInfo;

                if (booking == null && singleRoomBookings == null)
                {
                    groupInfo = new GroupInfoDTO { Name = group.Name, GroupSize = group.GroupSize };
                } 
                else if(booking != null)
                {
                    var room = context.Rooms.FirstOrDefault(r => r.Id == booking.RoomID);
                    groupInfo = new GroupInfoDTO { Name = group.Name, GroupSize = group.GroupSize, BookedRoom = room };
                }
                else
                {
                    var room = context.Rooms.FirstOrDefault(r => r.Id == singleRoomBookings.RoomID);
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

            using (var context = new SkyDbContext())
            {
                var weeks = context.Schedules
                    .Where(s => s.Id == 1)
                    .First().WeekInterval;
                var dayNumber = _dateConverter.ConvertDateToDaySequence(date, weeks);
                var scheduleWeek = _dateConverter.GetScheduleWeekNr(dayNumber);
                var weekDays = _dateConverter.GetWeekDays(scheduleWeek);
                var dates = new List<DateTime>();

                for (int i = 0; i < 7; i++)
                {
                    var individualDate = monday.AddDays(i);
                    var dateToFormattedString = individualDate.ToString("dddd", new CultureInfo("en-GB"));
                    dates.Add(individualDate);
                    weeksDates.Add(dateToFormattedString);
                }

                List<WeeklyGroupScheduleDTO> weeklyRoomSchedule = new List<WeeklyGroupScheduleDTO>();

                var dayCounter = 0;
                for (int i = weekDays.Min(); i < (weekDays.Min() + weekDays.Count); i++)
                {
                    
                    var bookings = context.Bookings
                        .Where(b => b.GroupID == groupId && b.DayNr == i)
                        .FirstOrDefault();

                    var singleRoomBookings = context.SingleRoomBookings
                        .Where(s => s.GroupID == groupId && s.Date == dates[dayCounter] && s.DayNr == i)
                        .FirstOrDefault();


                    string roomName = "Unbooked";
                    if (bookings != null)
                    {
                        roomName = context.Rooms.FirstOrDefault(r => r.Id == bookings.RoomID).Name;
                    }    
                    else if (singleRoomBookings != null)
                    {
                        roomName = context.Rooms.FirstOrDefault(r => r.Id == singleRoomBookings.RoomID).Name;
                    }

                    weeklyRoomSchedule.Add(new WeeklyGroupScheduleDTO
                    {
                        date = weeksDates[i - weekDays.Min()],
                        room = roomName,
                    });
                    dayCounter++;
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
    }
}
