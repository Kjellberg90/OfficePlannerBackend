using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTO;

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IRoomService _roomService;
        private readonly IDataAccess _dataAccess;

        public GroupService(IRoomService roomService, IDataAccess dataAccess)
        {
            _roomService = roomService;
            this._dataAccess = dataAccess;  
        }

        public Group GetGroup(int id)
        {
            var group = _dataAccess.ReadGroupsData()
                .Where(group => group.Id == id)
                .FirstOrDefault();

            return group;
        }

        public GroupInfoDTO GetGroupInfo(int id)
        {
            var group = GetGroup(id);

            if (group == null)
            {
                throw new Exception("GroupNotFoundException", new FileNotFoundException());
            }

            var groupInfo = new GroupInfoDTO()
            {
                Name = group.Name,
                Members = group.TeamMembers
            };

            if (group.BookedRoomNumber == null)
            {
                return groupInfo;
            }

            groupInfo.BookedRoom = _roomService.GetRoom(group.BookedRoomNumber);

            return groupInfo;

        }

        public void PrintGroupToFile(string data)
        {
            _dataAccess.PrintGroupToFile(data);
        }
    }
}
