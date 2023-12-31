﻿using DAL.Models;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.SingleBookingService
{
    public interface ISingleBookingService
    {
        public IEnumerable<SQLBooking> GetBookings();
        public void PostBookings();
        public void PostSingleBooking(SingleBookingDTO singleBookingDTO);
        public IEnumerable<UserDTO> GetSingleBookingsForDate(string date, int bookedRoomId);
        public void DeleteSingleBooking(DeleteSingleBookingDTO deleteSingleBooking);
    }
}
