using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DateConvert
    {
        public int ConvertDateToDaySequence(string date)
        {
            var defaultDate = new DateTime(2023, 01, 09);

            var requestedDate = DateTime.Parse(date);

            var daysDiff = (requestedDate - defaultDate).TotalDays;

            
            while(daysDiff > 21)
            {
                daysDiff = daysDiff -21;
            }

            return  ((int)daysDiff+1);
        }
    }
}
