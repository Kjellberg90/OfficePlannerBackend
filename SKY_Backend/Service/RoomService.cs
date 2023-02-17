using DAL;
using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomAccess _roomAccess;
        private readonly IBookingAccess _bookingAccess;
        private readonly IDateConverter _dateConverter;
        private readonly IGroupAccess _groupAccess;

        public RoomService(IRoomAccess roomAccess, IBookingAccess bookingAccess, IDateConverter dateConverter, IGroupAccess groupAccess)
        {
            _roomAccess = roomAccess;
            _bookingAccess = bookingAccess;
            _dateConverter = dateConverter;
            _groupAccess = groupAccess;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var booking = _bookingAccess.ReadBookingsData()
                .Where(b => b.DayNr == dayNr)
                .FirstOrDefault();

            if (booking == null)
            {
                throw new ArgumentNullException();
            }

            var roomsList = booking.Rooms;

            var roomInfoList = new List<RoomInfoDTO>();

            foreach (var room in roomsList)
            {

                var group = _groupAccess.ReadGroupsData()
                    .Where(g => g.Id == room.BookedBy)
                    .FirstOrDefault();

                int groupSize;
                string groupName;

                if (group == null)
                {
                    groupSize = 0;
                    groupName = string.Empty;
                }
                else
                {
                    groupSize = group.GroupSize;
                    groupName = group.Name;
                }

                int availableSeats = GetAvailableSeats(room, groupSize, date);

                roomInfoList.Add(new RoomInfoDTO
                {
                    RoomId = room.ID,
                    Name = room.Name,
                    Seats = room.Seats,
                    AvailableSeats = availableSeats < 0 ? 0 : availableSeats,
                    GroupName = groupName
                });
            }

            return roomInfoList.OrderBy(x => x.Name);
        }

        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(string date)
        {
            var formattedDate = _dateConverter.ConvertDateToDaySequence(date);
            var scheduleWeek = GetScheduleWeekNr(formattedDate);
            var weekDays = GetWeekDays(scheduleWeek, formattedDate);

            var bookings = new List<Booking>();

            foreach (var day in weekDays)
            {
                var booking = _bookingAccess.ReadBookingsData()
                    .Where(x => x.DayNr == day)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();

                if (booking == null)
                {
                    throw new Exception("Missing booking");
                }

                bookings.Add(booking);
            }

            var roomNames = _roomAccess.ReadRoomsData().Select(x => x.Name);
            var overviewList = new List<AdminRoomOverviewDTO>();

            foreach (var room in roomNames)
            {
                var roomName = room;
                var groupNames = new List<string>();
                for (int i = 0; i < bookings.Count; i++)
                {
                    var rooms = bookings[i].Rooms;

                    var groupId = rooms
                        .Where(x => x.Name == roomName)
                        .Select(x => x.BookedBy)
                        .FirstOrDefault();

                    var groupName = _groupAccess.ReadGroupsData()
                            .Where(x => x.Id == groupId)
                            .Select(x => x.Name)
                            .FirstOrDefault();

                    if (groupName == null)
                    {
                        groupNames.Add("");
                    }
                    else
                    {
                        groupNames.Add(groupName);
                    }
                }
                overviewList.Add(
                    new AdminRoomOverviewDTO
                    {
                        roomName = roomName,
                        groupNames = groupNames,
                    });
            }

            return overviewList;
        }

        public IEnumerable<AdminRoomDTO> AdminGetRooms()
        {
            var rooms = _roomAccess.ReadRoomsData();

            List<AdminRoomDTO> adminRoomList = new List<AdminRoomDTO>();

            foreach (var room in rooms) {

                adminRoomList.Add(
                    new AdminRoomDTO
                    {
                        Id = room.ID,
                        Name = room.Name,
                        Seats = room.Seats,
                    }
                    );
            }
            return adminRoomList;
        }

        public void AdminPostRoom(AdminPostRoomDTO adminAddRoom)
        {
            var roomsList = _roomAccess.ReadRoomsData();

            var newRoom = new Room()
            {
                ID = GetRoomId(),
                Name = adminAddRoom.Name,
                Seats = adminAddRoom.Seats,
                BookedBy = null
            };

            roomsList.Add( newRoom );

            _roomAccess.PrintRoomToFile(roomsList);

        }

        public void AdminDeleteRoom(AdminDeleteRoomDTO adminDeleteRoom)
        {
            var roomList = _roomAccess.ReadRoomsData();

            var roomToDelete = roomList.Where(x => x.ID == adminDeleteRoom.Id).FirstOrDefault();

            //AdminDeleteAllBookingsToConnectedToRoom(adminDeleteRoom);

            roomList.Remove(roomToDelete);

            _roomAccess.AdminDeleteRoom(roomList);
        }

        //public void AdminDeleteAllBookingsToConnectedToRoom(AdminDeleteRoomDTO adminDeleteRoom)
        //{
        //    var bookingList = _bookingAccess.ReadBookingsData();

        //    var bookingsToDelete = bookingList.Where(x => x.Rooms == adminDeleteRoom.Name)
        //}

        public void UpdateRoom(int roomId, AdminEditRoomDTO adminEditRoom)
        {
            var room = _roomAccess.ReadRoomsData()
                .Where(x => x.ID == roomId)
                .FirstOrDefault();

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            room.Name = adminEditRoom.Name;
            room.Seats = adminEditRoom.Seats;

            _roomAccess.UpdateRoomOnFile(room);
        }

        public int GetAvailableSeats(Room room, int groupSize, string date)
        {
            var singleBookings = _bookingAccess.ReadSingleBookingData()
                .Where(s => (s.Date == DateTime.Parse(date)) && (s.BookedRoom.ID == room.ID));

            var availableSeats = room.Seats - singleBookings.Count() - groupSize;

            return (availableSeats);
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
            var firstWeekDay = (7 * (week - 1) + 1);

            for (int i = firstWeekDay; i < (firstWeekDay + 7); i++)
            {
                list.Add(i);
            }

            return list;
        }

        public int GetRoomId()
        {
            var rooms = _roomAccess.ReadRoomsData();

            if (rooms?.Any() != true || rooms == null)
            {
                return 1;
            }

            var lastId = rooms
                .OrderBy(s => s.ID)
                .LastOrDefault()
                .ID;

            return lastId + 1;

        }

        public void Refresh()
        {
            _roomAccess.RefreshData();
        }
    }
}
