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
        public void PrintRoomToFile(List<Room> roomsList);
        public void DeleteRoomFromFile(int roomId);
        public void UpdateRoomOnFile(Room newRoomData);
        public void AdminDeleteRoom(List<Room> rooms);
        public void RefreshData();
    }
}