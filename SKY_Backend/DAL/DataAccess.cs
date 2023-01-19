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
    public class DataAccess : IDataAccess
    {
        public void PrintGroupToFile(string data)
        {
            MockData mockData = new MockData();

            var groupsJson = JsonSerializer.Serialize(mockData._groups);
            File.WriteAllText("JsonData/GroupsJson.txt", groupsJson);            
        }

        public List<Room> ReadRoomsData()
        {
            var roomsList = new List<Room>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/RoomJson.txt"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    roomsList = JsonSerializer.Deserialize<List<Room>>(json);

                    Console.WriteLine(json);

                    return roomsList;
                }
                return roomsList;
            }
        }

        public List<Group> ReadGroupsData()
        {
            var groupsList = new List<Group>();

            string json;

            using (StreamReader sr = new StreamReader("JsonData/GroupsJson.txt"))
            {
                while ((json = sr.ReadLine()) != null)
                {
                    groupsList = JsonSerializer.Deserialize<List<Group>>(json);

                    return groupsList;
                }
                return groupsList;
            }
        }

        public void PrintRoomToFile(Room newRoom)
        {
            var roomsList = ReadRoomsData();

            roomsList.Add(newRoom);

            using (StreamWriter sw = new StreamWriter("JsonData/RoomJson.txt"))
            {
                var roomJson = JsonSerializer.Serialize(roomsList);
                sw.WriteLine(roomJson);
            }
        }

        public void DeleteRoomFromFile(int roomId)
        {
            var roomsList = ReadRoomsData();
            roomsList
                .RemoveAll(room => room.ID == roomId);

            using (StreamWriter sw = new StreamWriter("JsonData/RoomJson.txt"))
            {
                var roomJson = JsonSerializer.Serialize(roomsList);
                sw.WriteLine(roomJson);
            }
        }

        public void UpdateRoomOnFile(Room newRoomData)
        {
            var roomsList = ReadRoomsData();
            
            var room = roomsList
                .Where(room => room.ID == newRoomData.ID)
                .FirstOrDefault();

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            roomsList.FirstOrDefault(room => room.ID == newRoomData.ID).Seats = newRoomData.Seats;
            roomsList.FirstOrDefault(room => room.ID == newRoomData.ID).Name = newRoomData.Name;
            roomsList.FirstOrDefault(room => room.ID == newRoomData.ID).BookedBy = newRoomData.BookedBy;

            using (StreamWriter sw = new StreamWriter("JsonData/RoomJson.txt"))
            {
                var roomJson = JsonSerializer.Serialize(roomsList);
                sw.WriteLine(roomJson);
            }
        }
    }
}
