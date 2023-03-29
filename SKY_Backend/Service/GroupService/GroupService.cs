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
using Service.DateHandler;

namespace Service.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly IDateHandler _dateHandler;
        private readonly IBookingAccess _bookingAccess;
        public GroupService(IDateHandler dateHandler, IBookingAccess bookingAccess)
        {
            _dateHandler = dateHandler;
            _bookingAccess = bookingAccess;
        }

        public GroupInfoDTO GetGroupInfo(string date, int groupId)
        {
            var dayNr = _dateHandler.ConvertDateToDaySequence(date);

            using (var context = new SkyDbContext())
            {
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
                else if (booking != null)
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

            var dayNumber = _dateHandler.ConvertDateToDaySequence(date);
            var scheduleWeek = _dateHandler.GetScheduleWeekNr(dayNumber);
            var weekDays = _dateHandler.GetWeekDays(scheduleWeek);
            var dates = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                var individualDate = monday.AddDays(i);
                var dateToFormattedString = individualDate.ToString("dddd", new CultureInfo("en-GB"));
                dates.Add(individualDate);
                weeksDates.Add(dateToFormattedString);
            }

            List<WeeklyGroupScheduleDTO> weeklyRoomSchedule = new List<WeeklyGroupScheduleDTO>();

            using (var context = new SkyDbContext())
            {
                var dayCounter = 0;
                for (int i = weekDays.Min(); i < weekDays.Min() + weekDays.Count; i++)
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
