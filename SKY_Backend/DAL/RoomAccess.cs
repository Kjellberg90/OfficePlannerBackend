﻿using System;
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

        public void PrintRoomToFile(Room newRoom)
        {
            var mockData = MockData.Instance._rooms;
            var roomsList = ReadRoomsData();

            roomsList.Add(newRoom);

            PrintToFile(mockData);
        }

        public void DeleteRoomFromFile(int roomId)
        {
            var roomsList = ReadRoomsData();
            roomsList
                .RemoveAll(room => room.ID == roomId);

            PrintToFile(roomsList);
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

            PrintToFile(roomsList);
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
    }
}
