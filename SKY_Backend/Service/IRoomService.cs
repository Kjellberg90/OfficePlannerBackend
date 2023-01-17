using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IRoomService
    {
        public IEnumerable<Room> GetRooms();

        public Room GetRoom(int? roomId);
        public IEnumerable<RoomInfoDTO> GetRoomsInfo();

    }
}