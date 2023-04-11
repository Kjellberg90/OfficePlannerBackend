using DAL;
using DAL.SQLModels;
using Service.DTO;
using Service.ScheduleService.ScheduleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class ScheduleService : IScheduleService
    {
        public int GetWeeksTotal(int scheduleId)
        {
            using (var context = new SkyDbContext())
            {
                var schedule = context.Schedules.First(s => s.Id == scheduleId);
                return schedule.WeekInterval;
            }
        }

        public IEnumerable<SQLSchedule> GetSchedules()
        {
            using (var context = new SkyDbContext())
            {
                var schedules = context.Schedules.ToList();
                return schedules;
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
}

