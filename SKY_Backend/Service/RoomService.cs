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
    public class RoomService : IRoomService
    {
        private readonly IRoomAccess _roomAccess;
        private readonly IBookingAccess _bookingAccess;

        public RoomService(IRoomAccess roomAccess, IBookingAccess bookingAccess)
        {
            _roomAccess = roomAccess;
            _bookingAccess = bookingAccess;
        }

        public IEnumerable<RoomInfoDTO> GetRoomsInfo()
        {
            var roomsList = _roomAccess.ReadRoomsData();

            var roomInfoList = new List<RoomInfoDTO>();

            foreach (var room in roomsList)
            {
                var group = MockData.Instance._groups
                    .Where(group => group.Id == room.BookedBy)
                    .FirstOrDefault();

                var groupSize = group.TeamMembers;

                roomInfoList.Add(new RoomInfoDTO
                {
                    Name = room.Name,
                    Seats = room.Seats,
                    AvailableSeats = room.Seats - groupSize
                });
            }

            return roomInfoList;
        }

        public Room GetRoom(int? roomId)
        {
            var roomsList = _roomAccess.ReadRoomsData();

            var room = roomsList
                .Where(room => room.ID == roomId)
                .FirstOrDefault();

            return room;
        }

        public IEnumerable<Room> GetRooms(string date)
        {
            var converter = new DateConvert();
            var dayNr = converter.ConvertDateToDaySequence(date);

            var booking = _bookingAccess.ReadBookingsData()
                .Where(b => b.DayNr == dayNr)
                .FirstOrDefault();



            return booking.Rooms;
        }

        public void PostRoomToFile(PostRoomDTO room)
        {
            //int newId;

            //var lastRoom = GetRooms()
            //    .OrderBy(room => room.ID)
            //    .LastOrDefault();

            //if (lastRoom == null)
            //{
            //    newId = 1;
            //}
            //else
            //{
            //    newId = lastRoom.ID + 1;
            //}


            //var newRoom = new Room()
            //{
            //    ID = newId,
            //    Name = room.Name,
            //    Seats = room.Seats,
            //    BookedBy = room.BookedBy
            //};

            //_roomAccess.PrintRoomToFile(newRoom);
        }

        public void DeleteRoom(int roomId)
        {
            _roomAccess.DeleteRoomFromFile(roomId);
        }

        public void UpdateRoom(Room room)
        {
            _roomAccess.UpdateRoomOnFile(room);
        }
    }
}
