using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingAccess _bookingAcces;

        public BookingService(IBookingAccess bookingService)
        {
            _bookingAcces = bookingService;
        }

        public IEnumerable<NewBooking> GetBookings()
        {
            return _bookingAcces.ReadBookingsData();
        }

        public void PostBookings()
        {
            _bookingAcces.PrintGroupToFile();
        }
    }
}
