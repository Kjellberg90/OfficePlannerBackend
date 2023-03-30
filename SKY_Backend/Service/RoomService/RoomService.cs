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
            using (var context = new SkyDbContext())
            {
                var weeks = context.Schedules
                    .Where(s => s.Id == 1)
                    .First().WeekInterval;
                var dayNr = _dateConverter.ConvertDateToDaySequence(date, weeks);
                var roomInfoList = new List<RoomInfoDTO>();
                var currentDate = DateTime.Parse(date);

                var rooms = GetRoomsList(dayNr, currentDate);
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

        public List<SQLRoom> GetRoomsList(int dayNr, DateTime date)
        {
            using (var context = new SkyDbContext())
            {
                var rooms = context.Rooms.ToList();
                var inspiredId = rooms.FirstOrDefault(r => r.Name == "Inspired").Id;
                var inspiredDivIds = rooms
                        .Where(r => r.Name == "Inspired A" || r.Name == "Inspired B")
                        .Select(r => r.Id)
                        .ToList();

                var inspiredBooked = CheckBookings(dayNr, new List<int> { inspiredId }, date);
                var inspiredDivBooked = CheckBookings(dayNr, inspiredDivIds, date);

                if (inspiredBooked)
                {
                    rooms = rooms.Where(r => !inspiredDivIds.Any(id => r.Id == id)).ToList();
                }
                else if (inspiredDivBooked)
                {
                    var inspired = rooms.Where(r => r.Id == inspiredId).First();
                    rooms.Remove(inspired);
                }

                return rooms;
            }
        }

        public bool CheckBookings(int dayNr, List<int> roomIds, DateTime date)
        {
            using (var context = new SkyDbContext())
            {
                foreach (var roomId in roomIds)
                {
                    var booking = context.Bookings
                        .Where(b => b.DayNr == dayNr && b.RoomID == roomId)
                        .FirstOrDefault();

                    var singleBookings = context.SingleBookings
                        .Where(b => b.Date == date && b.RoomID == roomId)
                        .FirstOrDefault();

                    if (booking != null || singleBookings != null)
                    {
                        return true;
                    }
                }
                return false;
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
    }
}
