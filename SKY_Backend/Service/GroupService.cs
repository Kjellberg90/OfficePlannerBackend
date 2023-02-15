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
        private readonly IGroupAccess _groupAccess;
        private readonly IDateConverter _dateConverter;
        private readonly IBookingAccess _bookingAccess;
        public GroupService(IGroupAccess groupAccess, IDateConverter dateConverter, IBookingAccess bookingAccess)
        {
            this._groupAccess = groupAccess;
            this._dateConverter = dateConverter;
            this._bookingAccess = bookingAccess;
        }

        public GroupInfoDTO GetGroupInfo(string date, int groupId)
        {
            var dayNr = _dateConverter.ConvertDateToDaySequence(date);
            var bookingsList = _bookingAccess.ReadBookingsData();
            var bookings = bookingsList.Where(x => x.DayNr == dayNr).FirstOrDefault();
            var roomInfo = bookings.Rooms.Where(i => i.BookedBy == groupId).FirstOrDefault();
            var groupInfo = _groupAccess.ReadGroupsData().Where(g => g.Id== groupId).FirstOrDefault();

            return new GroupInfoDTO { Name = groupInfo.Name, BookedRoom = roomInfo, GroupSize = groupInfo.GroupSize };
        }

        public IEnumerable<Group> GetGroups()
        {
            var groupList = _groupAccess.ReadGroupsData();

            return groupList;
        }

        public void UpdateGroup(int groupId, NewGroupInfoDTO newGroup)
        {
            var group = _groupAccess.ReadGroupsData()
                .Where(g => g.Id == groupId)
                .FirstOrDefault();

            if (group == null)
            {
                throw new Exception("Group not found");
            }

            group.Name = newGroup.Name;
            group.GroupSize = newGroup.GroupSize;
            group.Division = newGroup.Division;

            _groupAccess.PostUpdatedGroup(group);
        }
        public void DeleteGroup(int groupId)
        {
            _groupAccess.DeleteGroupFromFile(groupId);
        }

        public void Refresh()
        {
            _groupAccess.RefreshData();
        }
        public void AddGroup(AddGroupDTO addGroupDTO)
        {
            var groups = _groupAccess.ReadGroupsData();

            var newGroup = new Group()
            {
                Id = GetGroupId(),
                Name = addGroupDTO.Name,
                GroupSize = addGroupDTO.GroupSize,
                Division = addGroupDTO.Division
            };

            groups.Add(newGroup);
            _groupAccess.PrintToFile(groups);
        }

        public int GetGroupId()
        {
            var groups = _groupAccess.ReadGroupsData();

            if (groups?.Any() != true || groups == null)
            {
                return 1;
            }

            var lastId = groups
                .OrderBy(s => s.Id)
                .LastOrDefault()
                .Id;

            return lastId + 1;

        }
    }
}
