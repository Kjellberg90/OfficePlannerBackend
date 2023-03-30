using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DateHandlerService
{
    public interface IDatehandler
    {
        public int ConvertDateToDaySequence(string date, int weeksTotal);
        public int GetScheduleWeekNr(int dayNr);
        public List<int> GetWeekDays(int week);
    }
}
