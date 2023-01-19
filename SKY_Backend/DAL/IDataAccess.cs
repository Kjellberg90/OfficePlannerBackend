using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDataAccess
    {
        public List<Room> ReadRoomsData();
        public void PrintRoomToFile(Room data);
        public void DeleteRoomFromFile(int roomId);
        public void UpdateRoomOnFile(Room newRoomData);

        public List<Group> ReadGroupsData();
        public void PrintGroupToFile(string data);
    }
}
