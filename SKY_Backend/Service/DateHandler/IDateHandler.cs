using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DateHandler
{
    public interface IDateHandler
    {
        public int ConvertDateToDaySequence(string date);
        public int GetScheduleWeekNr(int dayNr);
        public List<int> GetWeekDays(int week);
    }
}
