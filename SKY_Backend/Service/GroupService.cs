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
            var roomInfo = bookings.Rooms.Where(i => i.ID == groupId).FirstOrDefault();
            var groupInfo = _groupAccess.ReadGroupsData().Where(g => g.Id== groupId).FirstOrDefault();

            return new GroupInfoDTO { Name = groupInfo.Name, BookedRoom = roomInfo, Members = groupInfo.TeamMembers };
        }

        public IEnumerable<string> GetGroups()
        {
            var groupList = _groupAccess.ReadGroupsData();

            var groupNames = groupList.Select(x => x.Name);

            return groupNames;
        }
    }
}
