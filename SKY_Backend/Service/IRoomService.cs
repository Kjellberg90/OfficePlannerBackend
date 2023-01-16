using DAL.Models;

namespace Service
{
    public interface IRoomService
    {
        public IEnumerable<Room> GetRooms();

        public Room GetRoom(int roomId);

    }
}