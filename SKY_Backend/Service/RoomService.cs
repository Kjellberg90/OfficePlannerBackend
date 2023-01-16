using DAL;
using DAL.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        public Room GetRoom(int? roomId)
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

        public IEnumerable<RoomInfoDTO> GetRoomsInfo()
        {
            IGroupService groupService = new GroupService();
            var allRooms = MockData.Instance._rooms;
            
            var roomInfoList = new List<RoomInfoDTO>();

            foreach (var room in allRooms)
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

        // Vi har BookedBy för att hämta antal platser som är upptagna
    }
}
