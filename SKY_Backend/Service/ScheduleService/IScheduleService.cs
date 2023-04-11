using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ScheduleService
{
    public interface IScheduleService
    {
        public int GetWeeksTotal(int scheduleId);
        public IEnumerable<SQLSchedule> GetSchedules();
        public void UpdateBookings(UpdateBookingsDTO[] updateBookingsDTO , int weekNr);
    }
}
