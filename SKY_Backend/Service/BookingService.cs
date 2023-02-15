using DAL;
using DAL.Models;
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

        public IEnumerable<Booking> GetBookings()
        {
            return _bookingAccess.ReadBookingsData();
        }

        public void PostBookings()
        {
            _bookingAccess.PrintGroupToFile();
        }

        public IEnumerable<UserDTO> GetSingleBookingsForDate(string date, int bookedRoomId)
        {
            var singleBookings = _bookingAccess.ReadSingleBookingData();

            var CurrentDate = DateTime.Parse(date);

            var singleBookingsOnDay = singleBookings.Where(x => x.Date == CurrentDate && x.BookedRoom.ID == bookedRoomId);

            List<UserDTO> users = new List<UserDTO>();

            foreach (var user in singleBookingsOnDay)
            {
                var userToAdd = new UserDTO()
                {
                    Id = user.Id,
                    UserName = user.Name
                };

                users.Add(userToAdd);
            }

            return users;
        }

        public void PostSingleBooking(SingleBookingDTO singleBookingDTO)
        {
            var bookedDate = DateTime.Parse(singleBookingDTO.Date);
            var room = _roomAccess.ReadRoomsData()
                .Where(r => r.ID == singleBookingDTO.RoomId)
                .FirstOrDefault();

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            var singleBooking = new SingleBooking()
            {
                Id = GetSingleBookingId(),
                Date = bookedDate,
                Name = singleBookingDTO.Name,
                BookedRoom = room
            };

            _bookingAccess.PostSingleBooking(singleBooking);
        }

        public void DeleteSingleBooking(DeleteSingleBookingDTO deleteSingleBooking)
        {
            var bookingList = _bookingAccess.ReadSingleBookingData();
            var date = DateTime.Parse(deleteSingleBooking.date);

            var singleBookingtoDelete = bookingList.Where(x => x.Name == deleteSingleBooking.userName && x.Date == date && x.BookedRoom.ID == deleteSingleBooking.roomId).FirstOrDefault();

            bookingList.Remove(singleBookingtoDelete);

            _bookingAccess.DeleteSingleBooking(bookingList);
        }

        public int GetSingleBookingId()
        {
            var singleBookings = _bookingAccess.ReadSingleBookingData();

            if (singleBookings?.Any() != true || singleBookings == null)
            {
                return 1;
            }

            var lastId = singleBookings
                .OrderBy(s => s.Id)
                .LastOrDefault()
                .Id;

            return lastId + 1;

        }
    }
}
