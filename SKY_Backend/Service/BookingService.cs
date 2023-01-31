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
        private readonly IBookingAccess _bookingAccess;

        public BookingService(IBookingAccess bookingService)
        {
            _bookingAccess = bookingService;
        }

        public IEnumerable<Booking> GetBookings()
        {
            return _bookingAccess.ReadBookingsData();
        }

        public void PostBookings()
        {
            _bookingAccess.PrintGroupToFile();
        }
    }
}
