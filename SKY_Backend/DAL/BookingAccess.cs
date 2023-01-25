using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class BookingAccess : IBookingAccess
    {
        public List<NewBooking> ReadBookingsData()
        {
            var groupsList = new List<NewBooking>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/Bookings.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupsList = JsonSerializer.Deserialize<List<NewBooking>>(json);

                    return groupsList;
                }
                return groupsList;
            }
        }


        public void PrintGroupToFile()
        {
            
        }


        private void PrintToFile(IEnumerable<object> objects)
        {
            string type = objects.FirstOrDefault().GetType().Name.ToString();

            string printDest = $"JsonData/Bookings.json";

            using (StreamWriter sw = new StreamWriter(printDest))
            {
                var json = JsonSerializer.Serialize(objects);
                sw.WriteLine(json);
            }
        }
    }
}
