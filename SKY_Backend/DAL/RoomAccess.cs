using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using DAL.Models;

namespace DAL
{
    public class RoomAccess : IRoomAccess
    {

        public List<Room> ReadRoomsData()
        {
            var roomsList = new List<Room>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/Rooms.json"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    roomsList = JsonSerializer.Deserialize<List<Room>>(json);

                    return roomsList;
                }
                return roomsList;
            }
        }

        public void PrintRoomToFile(List<Room> roomsList)
        {
            PrintToFile(roomsList);
        }

        public void DeleteRoomFromFile(int roomId)
        {
            var roomsList = ReadRoomsData();
            roomsList
                .RemoveAll(room => room.ID == roomId);

            PrintToFile(roomsList);
        }

        public void AdminDeleteRoom(List<Room> rooms)
        {
            PrintToFile(rooms);
        }

        public void UpdateRoomOnFile(Room newRoom)
        {
            var roomsList = ReadRoomsData();

            var room = roomsList
                .Where(room => room.ID == newRoom.ID)
                .FirstOrDefault();

                roomsList.FirstOrDefault(room => room.ID == newRoom.ID).Seats = newRoom.Seats;
                roomsList.FirstOrDefault(room => room.ID == newRoom.ID).Name = newRoom.Name;

            PrintToFile(roomsList.OrderBy(r => r.ID));
        }

        private void PrintToFile(IEnumerable<object> objects)
        {
            string type = objects.FirstOrDefault().GetType().Name.ToString();

            string printDest = $"JsonData/{type}s.json";

            using (StreamWriter sw = new StreamWriter(printDest))
            {
                var json = JsonSerializer.Serialize(objects);
                sw.WriteLine(json);
            }
        }

        public void RefreshData()
        {
            var mock = new MockData();
            PrintToFile(mock._rooms);
        }
    }
}
