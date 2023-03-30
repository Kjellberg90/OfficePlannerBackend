﻿using DAL.Models;
using Service.DTO;

namespace Service
{
    public interface IRoomService
    {
        public IEnumerable<RoomInfoDTO> GetRoomsInfo(string date);
        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(int weekNr, int scheduleId);
        public IEnumerable<AdminRoomOverviewDTO> AdminRoomsOverview(string date, int scheduleId);
        public IEnumerable<AdminRoomDTO> AdminGetRooms();
        public void AdminDeleteRoom(AdminDeleteRoomDTO adminDeleteRoom);
        public void AdminPostRoom(AdminPostRoomDTO adminAddRoom);
        public void UpdateRoom(int roomId, AdminEditRoomDTO adminEditRoom);

    }
}