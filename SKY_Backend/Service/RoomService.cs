using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        public Room GetRoom(int roomId)
        {
            var room = MockData.Instance._rooms
                .Where(room => room.ID == roomId)
                .FirstOrDefault();

            return room;
        }

        public IEnumerable<Room> GetRooms()
        {
            var newList = MockData.Instance._rooms;
            return newList;
        }
    }
}
