using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IRoomService
    {
        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date);

    }
}