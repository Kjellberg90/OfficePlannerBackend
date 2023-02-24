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

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupAccess _groupAccess;
        private readonly IDateConverter _dateConverter;
        private readonly IBookingAccess _bookingAccess;
        public GroupService(IGroupAccess groupAccess, IDateConverter dateConverter, IBookingAccess bookingAccess)
        {
            this._groupAccess = groupAccess;
            this._dateConverter = dateConverter;
            this._bookingAccess = bookingAccess;
        }

        public GroupInfoDTO GetGroupInfo(string date, int groupId)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var bookingsList = _bookingAccess.ReadBookingsData();
            var bookings = bookingsList.Where(x => x.DayNr == dayNr).FirstOrDefault();
            var roomInfo = bookings.Rooms.Where(i => i.BookedBy == groupId).FirstOrDefault();
            var groupInfo = _groupAccess.ReadGroupsData().Where(g => g.Id == groupId).FirstOrDefault();

            return new GroupInfoDTO { Name = groupInfo.Name, BookedRoom = roomInfo, GroupSize = groupInfo.GroupSize };
        }

        public IEnumerable<Group> GetGroups()
        {
            var groupList = _groupAccess.ReadGroupsData();

            return groupList;
        }

        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup)
        {
            var group = _groupAccess.ReadGroupsData()
                .Where(g => g.Id == groupId)
                .FirstOrDefault();

            if (group == null)
            {
                throw new Exception("Group not found");
            }

            group.Name = newGroup.Name;
            group.GroupSize = newGroup.GroupSize;
            group.Division = newGroup.Division;

            _groupAccess.PostUpdatedGroup(group);
        }
        public void DeleteGroup(int groupId)
        {
            _groupAccess.DeleteGroupFromFile(groupId);
        }

        public void Refresh()
        {
            _groupAccess.RefreshData();
        }
        public void AddGroup(AddGroupDTO addGroupDTO)
        {
            var groups = _groupAccess.ReadGroupsData();

            var newGroup = new Group()
            {
                Id = GetGroupId(),
                Name = addGroupDTO.Name,
                GroupSize = addGroupDTO.GroupSize,
                Division = addGroupDTO.Division
            };

            groups.Add(newGroup);
            _groupAccess.PrintToFile(groups);
        }

        public int GetGroupId()
        {
            var groups = _groupAccess.ReadGroupsData();

            if (groups?.Any() != true || groups == null)
            {
                return 1;
            }

            var lastId = groups
                .OrderBy(s => s.Id)
                .LastOrDefault()
                .Id;

            return lastId + 1;

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


            List<Booking> weeklyBookings = new List<Booking>();
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

            foreach (var day in weekDays)
            {
                var booking = _bookingAccess.ReadBookingsData()
                    .Where(x => x.DayNr == day)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();

                if (booking == null)
                {
                    throw new Exception("Missing booking");
                }

                weeklyBookings.Add(booking);
            }

            List<Room> roomList = new List<Room>();

            foreach (var day in weeklyBookings)
            {
                var bookedRoom = day.Rooms.Where(x => x.BookedBy == groupId).FirstOrDefault();
                roomList.Add(bookedRoom);
            }

            List<WeeklyGroupScheduleDTO> weeklyRoomSchedule = new List<WeeklyGroupScheduleDTO>();

            for (int i = 0; i < 7; i++)
            {
                if (roomList[i] != null)
                {
                    var individualday = new WeeklyGroupScheduleDTO()
                    {
                        date = weeksDates[i],
                        room = roomList[i].Name,
                    };
                    weeklyRoomSchedule.Add(individualday);
                }
                else
                {
                    var individualday = new WeeklyGroupScheduleDTO()
                    {
                        date = weeksDates[i],
                        room = "Unbooked",
                    };
                    weeklyRoomSchedule.Add(individualday);
                }
            }
            return weeklyRoomSchedule;
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
