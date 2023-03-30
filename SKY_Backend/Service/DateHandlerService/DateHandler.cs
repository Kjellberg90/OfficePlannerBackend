using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DateHandlerService
{
    public class DateHandler : IDatehandler
    {
        public int ConvertDateToDaySequence(string date, int weeksTotal)
        {
            var defaultDate = new DateTime(2023, 01, 08);

            var requestedDate = DateTime.Parse(date);

            var daysDiff = (requestedDate - defaultDate).TotalDays;

            var totalDaysInSchedule = weeksTotal * 7;


            while (daysDiff > totalDaysInSchedule)
            {
                daysDiff = daysDiff - totalDaysInSchedule;
            }

            return (int)daysDiff;
        }
        public int GetScheduleWeekNr(int dayNr)
        {
            if (dayNr == 0) { throw new Exception("Incorrect day number"); }

            decimal value = dayNr / 7m;
            var week = Convert.ToInt32(Math.Ceiling(value));

            return week;
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
