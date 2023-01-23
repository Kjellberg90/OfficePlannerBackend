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
        private readonly IGroupAccess _dataAccess;

        public GroupService(IRoomService roomService, IGroupAccess dataAccess)
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

        public IEnumerable<Group> GetGroups()
        {
            return _dataAccess.ReadGroupsData();
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

        public void PostGroup(PostGroupDTO data)
        {
            int newId;


            var lastGroup = GetGroups()
                .OrderBy(group => group.Id)
                .LastOrDefault();

            newId = lastGroup == null ? 1 : lastGroup.Id + 1;
            data.BookedRoomId = data.BookedRoomId > 0 ? data.BookedRoomId : null;

            var newGroup = new Group()
            {
                Id = newId,
                Name = data.Name,
                TeamMembers = data.TeamMembers,
                BookedRoomNumber = data.BookedRoomId
            };

            _dataAccess.PrintGroupToFile(newGroup);
        }

        public void DeleteGroup(int id)
        {
            _dataAccess.DeleteGroupFromFile(id);
        }

        public void UpdateGroup(Group group)
        {
            _dataAccess.UpdateGroupOnFile(group);
        }
    }
}
