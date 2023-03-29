using DAL;
using DAL.Models;
using DAL.SQLModels;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IDateConverter _dateConverter;

        public RoomService(IDateConverter dateConverter)
        {
            _dateConverter = dateConverter;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var roomInfoList = new List<RoomInfoDTO>();
            var currentDate = DateTime.Parse(date);

            using (var context = new SkyDbContext())
            {
                var rooms = context.Rooms.ToList();
                var bookings = context.Bookings
                    .Where(b => b.DayNr == dayNr)
                    .ToList();

                var singleRoomBookings = context.SingleRoomBookings
                    .Where(s => s.DayNr == dayNr)
                    .ToList();

                foreach (var room in rooms)
                {
                    var booking = bookings.Find(b => b.RoomID == room.Id);
                    var singleRoomBooking = singleRoomBookings.Find(b => b.RoomID == room.Id);

                    SQLGroup? group = null;

                    if (booking != null)
                    {
                        group = context.Groups.FirstOrDefault(g => g.Id == booking.GroupID);
                    }
                    else if (singleRoomBooking != null)
                    {
                        group = context.Groups.FirstOrDefault(g => g.Id == singleRoomBooking.GroupID);
                    }

                    var groupSize = group == null ? 0 : group.GroupSize;

                    var availableSeats = GetAvailableSeats(room, groupSize, date);

                    var singelBookings = context.SingleBookings
                        .Where(s => s.Date == currentDate && s.RoomID == room.Id)
                        .ToList()
                        .Count();

                    roomInfoList.Add(new RoomInfoDTO
                    {
                        Name = room.Name,
                        RoomId = room.Id,
                        Seats = room.Seats,
                        GroupName = group == null ? "" : group.Name,
                        AvailableSeats = availableSeats >= 0 ? availableSeats : 0,
                        SingleBookings = singelBookings
                    });

                }

                return roomInfoList;
            }
        }

        public int GetAvailableSeats(SQLRoom room, int groupSize, string date)
        {
            using (var context = new SkyDbContext())
            {
                var singleBookings = context.SingleBookings
                    .Where(s => s.Date == DateTime.Parse(date) && s.RoomID == room.Id);

                var availableSeats = room.Seats - singleBookings.Count() - groupSize;
                return availableSeats;
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

        public List<int> GetWeekDays(int week, int dayNr)
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
