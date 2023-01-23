using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRoomAccess
    {
        public List<Room> ReadRoomsData();
        public void PrintRoomToFile(Room data);
        public void DeleteRoomFromFile(int roomId);
        public void UpdateRoomOnFile(Room newRoomData);
    }
}