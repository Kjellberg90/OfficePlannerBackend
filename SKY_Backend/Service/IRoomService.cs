using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IRoomService
    {
        public Room GetRoom(int? roomId);
        public IEnumerable<Room> GetRooms(string date);
        public IEnumerable<RoomInfoDTO> GetRoomsInfo();
        public void PostRoomToFile(PostRoomDTO room);
        public void DeleteRoom(int roomId);
        public void UpdateRoom(Room room);
    }
}