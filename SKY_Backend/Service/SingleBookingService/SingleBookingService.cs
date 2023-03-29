using DAL;
using DAL.Models;
using DAL.SQLModels;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.BookingService
{
    public class SingleBookingService : ISingleBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IDateConverter _dateConverter;

        public SingleBookingService(IBookingAccess bookingAcess, IDateConverter dateConverter)
        {
            _bookingAccess = bookingAcess;
            _dateConverter = dateConverter;
        }

        public IEnumerable<SQLBooking> GetBookings()
        {
            using (var context = new SkyDbContext())
            {
                var bookings = context.Bookings.ToList();
                return bookings;
            }

        }

        public void PostBookings()
        {
            _bookingAccess.PrintGroupToFile();
        }

        public IEnumerable<UserDTO> GetSingleBookingsForDate(string date, int bookedRoomId)
        {
            using (var context = new SkyDbContext())
            {
                var singleBookings = context.SingleBookings.ToList();
                var currentDate = DateTime.Parse(date);
                var singleBookingsOnDay = singleBookings.Where(x => x.Date == currentDate && x.RoomID == bookedRoomId);

                var users = new List<UserDTO>();

                foreach (var user in singleBookingsOnDay)
                {
                    users.Add(new UserDTO
                    {
                        Id = user.ID,
                        UserName = user.Name
                    });
                }

                return users;
            }
        }

        public void PostSingleBooking(SingleBookingDTO singleBookingDTO)
        {
            using (var context = new SkyDbContext())
            {
                int pinInt = singleBookingDTO.PinNumbers
                    .Select((t, i) => t * Convert.ToInt32(Math.Pow(10, singleBookingDTO.PinNumbers.Count - i - 1)))
                    .Sum();

                var bookedDate = DateTime.Parse(singleBookingDTO.Date);
                var room = context.Rooms
                    .Where(r => r.Id == singleBookingDTO.RoomId)
                    .FirstOrDefault();

                if (room == null) throw new Exception("Room not found");

                context.SingleBookings.Add(new SQLSingleBooking
                {
                    Name = singleBookingDTO.Name,
                    Date = bookedDate,
                    RoomID = singleBookingDTO.RoomId,
                    PinCode = pinInt
                });

                context.SaveChanges();
            }
        }

        public void DeleteSingleBooking(DeleteSingleBookingDTO deleteSingleBooking)
        {
            using (var context = new SkyDbContext())
            {
                var date = DateTime.Parse(deleteSingleBooking.date);
                var singleBookingToDelete = context.SingleBookings
                    .Where(x => x.Name == deleteSingleBooking.userName && x.Date == date && x.RoomID == deleteSingleBooking.roomId)
                    .FirstOrDefault();

                if (singleBookingToDelete == null) throw new Exception("SingleBooking not found");
                var loggedPin = singleBookingToDelete.PinCode;
                var pinArray = loggedPin.ToString().Select(o => Convert.ToInt32(o) - 48).ToList();

                for (int i = 0; i < pinArray.Count; i++)
                {
                    if (pinArray[i] != deleteSingleBooking.pinNumbers[i])
                    {
                        throw new ArgumentException("Incorrect PIN");
                    }
                }



                context.SingleBookings.Remove(singleBookingToDelete);
                context.SaveChanges();
            }
        }

        public int GetScheduleWeekNr(int dayNr)
        {
            if (dayNr == 0) { throw new Exception("Incorrect day number"); }

            if (dayNr >= 1 && dayNr < 8)
            {
                return 1;
            }
            else if (dayNr >= 8 && dayNr < 15)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public List<int> GetWeekDays(int week)
        {
            var list = new List<int>();
            var firstWeekDay = 7 * (week - 1) + 1;

            for (int i = firstWeekDay; i < firstWeekDay + 5; i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}