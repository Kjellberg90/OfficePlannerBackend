using DAL.Models;
using Service.DTO;

namespace Service.RoomService
{
    public interface IRoomService
    {
        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date);
    }
}