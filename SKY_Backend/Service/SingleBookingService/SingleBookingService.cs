using DAL;
using DAL.Models;
using DAL.SQLModels;
using Microsoft.EntityFrameworkCore;
using Service.DateHandlerService;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.SingleBookingService
{
    public class SingleBookingService : ISingleBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IDatehandler _dateConverter;

        public SingleBookingService(IBookingAccess bookingAcess, IDatehandler dateConverter)
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
                    Password = singleBookingDTO.Password
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
                    .Where(x => x.Name == deleteSingleBooking.name && x.Date == date && x.RoomID == deleteSingleBooking.roomId)
                    .FirstOrDefault();

                if (singleBookingToDelete == null) throw new Exception("SingleBooking not found");
                var loggedPassword = singleBookingToDelete.Password;

                if (loggedPassword != deleteSingleBooking.password)
                {
                    throw new Exception("Incorrect password");
                }

                context.SingleBookings.Remove(singleBookingToDelete);
                context.SaveChanges();
            }
        }






    }
}