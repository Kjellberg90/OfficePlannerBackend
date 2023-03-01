using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class BookingAccess : IBookingAccess
    {
        //private readonly IRoomAccess _roomAccess;

        //public BookingAccess(IRoomAccess roomAcces)
        //{
        //    _roomAccess = roomAcces;
        //}

        public void PostSingleBooking(SingleBooking singleBooking)
        {
            var singleBookingsList = ReadSingleBookingData();

            singleBookingsList.Add(singleBooking);

            PrintToFile(singleBookingsList);
        }



        public void PrintGroupToFile()
        {
            var list = ReadBookingsData();
            
            var orderedList = list.OrderBy(x => x.Id);

            PrintToFile(orderedList);
        }

        
        public List<Booking> ReadBookingsData()
        {
            var groupsList = new List<Booking>();

            string json;

            using (StreamReader sr = new StreamReader($"JsonData/Bookings.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupsList = JsonSerializer.Deserialize<List<Booking>>(json);

                    return groupsList;
                }
                return groupsList;
            }
        }

        public List<SingleBooking> ReadSingleBookingData()
        {
            var groupsList = new List<SingleBooking>();

            string json;

            using (StreamReader sr = new StreamReader($"JsonData/SingleBookings.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupsList = JsonSerializer.Deserialize<List<SingleBooking>>(json);

                    return groupsList;
                }
                return groupsList;
            }
        }

        public void DeleteSingleBooking(List<SingleBooking> bookingList)
        {
            PrintToFile(bookingList);
        }


        private void PrintToFile(IEnumerable<object> objects)
        {
            string type = objects.FirstOrDefault().GetType().Name.ToString();

            string printDest = $"JsonData/{type}s.json";

            using (StreamWriter sw = new StreamWriter(printDest))
            {
                var json = JsonSerializer.Serialize(objects);
                sw.WriteLine(json);
            }
        }
    }
}
