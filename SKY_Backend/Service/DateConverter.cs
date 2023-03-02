using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DateConverter : IDateConverter
    {
        public int ConvertDateToDaySequence(string date)
        {
            var defaultDate = new DateTime(2023, 01, 08);

            var requestedDate = DateTime.Parse(date);

            var daysDiff = (requestedDate - defaultDate).TotalDays;

            
            while(daysDiff >= 22)
            {
                daysDiff = daysDiff -21;
            }

            return  ((int)daysDiff);
        }
    }
}
