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
        public Group GetGroup(int id)
        {
            var group = MockData.Instance._groups
                .Where(group => group.Id == id)
                .FirstOrDefault();

            return group;
        }
        public IEnumerable<Group> GetGroups()
        {
            var newList = MockData.Instance._groups;
            return newList;
        }

        public GroupInfoDTO GetGroupInfo(int id)
        {
            IRoomService roomService = new RoomService();

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

            if(group.BookedRoomNumber == null )
            {
                return groupInfo;
            }

            groupInfo.BookedRoom = roomService.GetRoom(group.BookedRoomNumber);

            return groupInfo;

        }
    }
}
