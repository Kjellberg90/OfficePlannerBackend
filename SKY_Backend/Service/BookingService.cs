using DAL;
using DAL.Models;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingAccess _bookingAccess;
        private readonly IRoomAccess _roomAccess;
        private readonly IDateConverter _dateConverter;

        public BookingService(IBookingAccess bookingAcess, IRoomAccess roomAccess, IDateConverter dateConverter)
        {
            _bookingAccess = bookingAcess;
            _roomAccess = roomAccess;
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
                    RoomID = singleBookingDTO.RoomId
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

                context.SingleBookings.Remove(singleBookingToDelete); 
                context.SaveChanges();
            }
        }

        public void RefreshBookings()
        {
            using (var context = new SkyDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var data = _bookingAccess.ReadBookingsData();

                foreach (var booking in data)
                {
                    var roomsList = booking.Rooms;

                    foreach (var room in roomsList)
                    {
                        if (room.BookedBy != null)
                        {
                            context.Bookings.Add(
                            new SQLBooking
                            {
                                DayNr = booking.DayNr,
                                GroupID = (int)room.BookedBy,
                                RoomID = context.Rooms.Where(x => x.Name == room.Name).FirstOrDefault().Id,
                                ScheduleID = 1
                            }
                            );
                        }

                    }
                }

                context.SaveChanges();


                var rooms = context.Rooms.ToArray();
                var groups = context.Groups.ToArray();
                var schedules = context.Schedules.ToArray();
                var bookings = context.Bookings.ToArray();
                foreach (var group in groups)
                {
                    Console.WriteLine("Group: " + group.Name);
                }
                foreach (var room in rooms)
                {
                    Console.WriteLine("Room: " + room.Name);
                }
                foreach (var schedule in schedules)
                {
                    Console.WriteLine("Schedule: " + schedule.Name);
                }
                foreach (var booking in bookings)
                {
                    Console.WriteLine("Bookings ID: " + booking.Id);
                    Console.WriteLine("Bookings DayNr: " + booking.DayNr);
                    Console.WriteLine("Bookings: RoomId" + booking.RoomID);

                }
            }
        }
    }
}