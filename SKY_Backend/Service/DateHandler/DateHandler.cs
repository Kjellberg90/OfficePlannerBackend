using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DateHandler
{
    public class DateHandler : IDateHandler
    {
        public int ConvertDateToDaySequence(string date)
        {
            var defaultDate = new DateTime(2023, 01, 08);

            var requestedDate = DateTime.Parse(date);

            var daysDiff = (requestedDate - defaultDate).TotalDays;


            while (daysDiff >= 22)
            {
                daysDiff = daysDiff - 21;
            }

            return (int)daysDiff;
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
