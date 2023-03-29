using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AdminRomService
{
    public interface IAdminRoomService
    {
        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(string date);
        public void AdminDeleteRoom(AdminDeleteRoomDTO adminDeleteRoom);
        public void AdminPostRoom(AdminPostRoomDTO adminAddRoom);
        public void UpdateRoom(int roomId, AdminEditRoomDTO adminEditRoom);
        public IEnumerable<AdminRoomDTO> AdminGetRooms();
    }
}
