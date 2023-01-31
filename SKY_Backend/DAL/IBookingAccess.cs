using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IBookingAccess
    {
        public void PrintGroupToFile();
        public List<Booking> ReadBookingsData();
        public void PostSingleBooking(SingleBooking singleBooking);
    }
}
