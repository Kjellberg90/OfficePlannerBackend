using DAL;
using DAL.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
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
    }
}
