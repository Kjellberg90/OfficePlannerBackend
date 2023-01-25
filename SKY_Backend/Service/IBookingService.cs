using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBookingService
    {
        public IEnumerable<NewBooking> GetBookings();
        public void PostBookings();
    }
}
