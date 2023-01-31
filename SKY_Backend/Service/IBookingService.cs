using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBookingService
    {
        public IEnumerable<Booking> GetBookings();
        public void PostBookings();
        public void PostSingleBooking(SingleBookingDTO singleBookingDTO);
    }
}
